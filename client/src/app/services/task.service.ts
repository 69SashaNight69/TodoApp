import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { CreateTaskDto, PagedTasksDto, TaskDto, UpdateTaskDto, TaskQueryParameters } from '../models/task.models';

@Injectable({
  providedIn: 'root'
})
export class TaskService {
  private apiUrl = 'https://localhost:7090/api/tasks';

  constructor(private http: HttpClient) { }

  getTasks(query: TaskQueryParameters): Observable<PagedTasksDto> {
    let params = new HttpParams()
      .set('pageNumber', query.pageNumber.toString())
      .set('pageSize', query.pageSize.toString());

    if (query.searchTerm) {
      params = params.set('searchTerm', query.searchTerm);
    }
    if (query.categoryId) {
      params = params.set('categoryId', query.categoryId);
    }

    return this.http.get<PagedTasksDto>(this.apiUrl, { params });
  }

  getById(id: string): Observable<TaskDto> {
    return this.http.get<TaskDto>(`${this.apiUrl}/${id}`);
  }

  createTask(dto: CreateTaskDto): Observable<TaskDto> {
    return this.http.post<TaskDto>(this.apiUrl, dto);
  }

  updateTask(id: string, dto: UpdateTaskDto): Observable<TaskDto> {
    return this.http.put<TaskDto>(`${this.apiUrl}/${id}`, dto);
  }

  deleteTask(id: string): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`);
  }
}
