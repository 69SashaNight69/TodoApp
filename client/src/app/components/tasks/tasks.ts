import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { TaskService } from '../../services/task.service';
import { CategoryService } from '../../services/category.service';
import { AuthService } from '../../services/auth.service';
import { TaskDto, CreateTaskDto, UpdateTaskDto, TaskQueryParameters } from '../../models/task.models';
import { CategoryDto, CreateCategoryDto } from '../../models/category.models';

@Component({
  selector: 'app-tasks',
  imports: [CommonModule, FormsModule],
  templateUrl: './tasks.html',
  styleUrl: './tasks.css'
})
export class Tasks implements OnInit {
  tasks: TaskDto[] = [];
  categories: CategoryDto[] = [];

  totalCount = 0;
  pageSizeOptions = [5, 10, 20];
  query: TaskQueryParameters = {
    searchTerm: '',
    categoryId: '',
    pageNumber: 1,
    pageSize: 5
  };

  newTask: CreateTaskDto = { title: '', description: '', dueDate: '', categoryId: '' };
  newCategory: CreateCategoryDto = { name: '' };

  errorMessage: string | null = null;
  userEmail: string | null = '';

  constructor(
    private taskService: TaskService,
    private categoryService: CategoryService,
    private authService: AuthService,
    private router: Router
  ) { }

  ngOnInit(): void {
    this.userEmail = localStorage.getItem('userEmail');
    this.loadCategories();
    this.loadTasks();
  }

  loadCategories(): void {
    this.categoryService.getCategories().subscribe({
      next: (data) => this.categories = data,
      error: (err) => this.errorMessage = err.error?.message || 'Помилка завантаження категорій.'
    });
  }

  loadTasks(): void {
    this.taskService.getTasks(this.query).subscribe({
      next: (data) => {
        this.tasks = data.items;
        this.totalCount = data.totalCount;
      },
      error: (err) => this.errorMessage = err.error?.message || 'Помилка завантаження завдань.'
    });
  }

  createTask(): void {
    this.errorMessage = null;
    if (!this.newTask.title.trim()) return;

    const dto: CreateTaskDto = {
      title: this.newTask.title,
      description: this.newTask.description ? this.newTask.description : undefined,
      dueDate: this.newTask.dueDate ? this.newTask.dueDate : undefined,
      categoryId: this.newTask.categoryId ? this.newTask.categoryId : undefined
    };

    this.taskService.createTask(dto).subscribe({
      next: () => {
        this.newTask = { title: '', description: '', dueDate: '', categoryId: '' };
        this.loadTasks();
      },
      error: (err) => this.errorMessage = err.error?.message || 'Помилка створення завдання.'
    });
  }

  createCategory(): void {
    this.errorMessage = null;
    if (!this.newCategory.name.trim()) return;

    this.categoryService.createCategory(this.newCategory).subscribe({
      next: () => {
        this.newCategory.name = '';
        this.loadCategories();
      },
      error: (err) => this.errorMessage = err.error?.message || 'Помилка створення категорії.'
    });
  }

  toggleComplete(task: TaskDto): void {
    const updateDto: UpdateTaskDto = {
      title: task.title,
      description: task.description,
      isCompleted: !task.isCompleted,
      dueDate: task.dueDate,
      categoryId: task.category?.id
    };

    this.taskService.updateTask(task.id, updateDto).subscribe({
      next: () => this.loadTasks(),
      error: (err) => this.errorMessage = err.error?.message || 'Помилка оновлення завдання.'
    });
  }

  deleteTask(id: string): void {
    if (confirm('Ви впевнені, що хочете видалити це завдання?')) {
      this.taskService.deleteTask(id).subscribe({
        next: () => this.loadTasks(),
        error: (err) => this.errorMessage = err.error?.message || 'Помилка видалення завдання.'
      });
    }
  }

  deleteCategory(id: string, event: Event): void {
    event.stopPropagation();
    if (confirm('Ви видаляєте категорію. Завдання в цій категорії залишаться, але стануть безкатегорійними. Продовжити?')) {
      this.categoryService.deleteCategory(id).subscribe({
        next: () => {
          if (this.query.categoryId === id) {
            this.query.categoryId = '';
          }
          this.loadCategories();
          this.loadTasks();
        },
        error: (err) => this.errorMessage = err.error?.message || 'Помилка видалення категорії.'
      });
    }
  }

  onSearch(): void {
    this.query.pageNumber = 1;
    this.loadTasks();
  }

  filterByCategory(categoryId: string): void {
    this.query.categoryId = categoryId;
    this.query.pageNumber = 1;
    this.loadTasks();
  }

  nextPage(): void {
    if (this.query.pageNumber * this.query.pageSize < this.totalCount) {
      this.query.pageNumber++;
      this.loadTasks();
    }
  }

  prevPage(): void {
    if (this.query.pageNumber > 1) {
      this.query.pageNumber--;
      this.loadTasks();
    }
  }

  changePageSize(size: number): void {
    this.query.pageSize = size;
    this.query.pageNumber = 1;
    this.loadTasks();
  }

  logout(): void {
    this.authService.logout();
    this.router.navigate(['/login']);
  }
}
