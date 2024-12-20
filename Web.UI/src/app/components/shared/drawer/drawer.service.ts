import {AppRoles} from '../../../core/config/configuration';
import {AuthService} from '../../../core/services/auth.service';
import {Injectable} from '@angular/core';

@Injectable({providedIn: 'root'})
export class DrawerService {
  AdminActions: DrawerAction[] = [
    {Id: 0, title: 'Dashboard', icon: 'bi bi-columns-gap fs-4 me-2', path: 'admin/dashboard'},
    {Id: 1, title: 'Subjects', icon: 'bi bi-journal-bookmark-fill fs-4 me-2', path: 'admin/subjects'},
    {Id: 2, title: 'Students', icon: 'bi bi-people fs-4 me-2', path: 'admin/students'},
    {Id: 3, title: 'Exams', icon: 'bi bi-file-earmark-text fs-4 me-2', path: 'admin/exams-results'},
  ];

  StudentActions: DrawerAction[] = [
    {Id: 0, title: 'Dashboard', icon: 'bi bi-columns-gap fs-4 me-2', path: 'student/dashboard'},
    {Id: 1, title: 'Subjects', icon: 'bi bi-journal-bookmark-fill fs-4 me-2', path: 'student/subjects'},
    {Id: 2, title: 'Profile', icon: 'bi bi-person fs-4 me-2', path: 'student/profile'},
  ];

  constructor(private authService: AuthService) {
  }

  get GetActions(): DrawerAction[] {
    switch (this.authService.UserRole()) {
      case AppRoles.Admin:
        return this.AdminActions;
      case AppRoles.Student:
        return this.StudentActions;
      default:
        return [];
    }
  }
}

export interface DrawerAction {
  Id: number;
  title: string;
  icon: string;
  path: string;
}

