import { CategoryDto } from './category.models';

export interface TaskDto {
  id: string;
  title: string;
  description?: string;
  isCompleted: boolean;
  dueDate?: string;
  createdAt: string;
  category?: CategoryDto;
}

export interface CreateTaskDto {
  title: string;
  description?: string;
  dueDate?: string;
  categoryId?: string;
}

export interface UpdateTaskDto {
  title: string;
  description?: string;
  isCompleted: boolean;
  dueDate?: string;
  categoryId?: string;
}

export interface PagedTasksDto {
  items: TaskDto[];
  totalCount: number;
}
