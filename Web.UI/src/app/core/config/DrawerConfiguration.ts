import {DrawerAction} from '../../app.component';
import {Injectable} from '@angular/core';

@Injectable({providedIn: 'root'})
export class DrawerConfiguration {
  AdminActions: DrawerAction[] = [
    {title: 'Dashboard', icon: 'bi bi-columns-gap fs-4 me-2', path: 'admin/dashboard'},
    {title: 'Subjects', icon: 'bi bi-journal-bookmark-fill fs-4 me-2', path: 'admin/subjects'},
    {title: 'Students', icon: 'bi bi-people fs-4 me-2', path: 'admin/students'},
    {title: 'Exams', icon: 'bi bi-file-earmark-text fs-4 me-2', path: 'admin/exams-results'},
  ];

  StudentActions: DrawerAction[] = [
    {title: 'Dashboard', icon: 'bi bi-columns-gap fs-4 me-2', path: 'student/dashboard'},
    {title: 'Subjects', icon: 'bi bi-journal-bookmark-fill fs-4 me-2', path: 'student/subjects'},
    {title: 'Profile', icon: 'bi bi-person fs-4 me-2', path: 'student/profile'},
  ];

  get Admin() {
    return this.AdminActions;
  }

  get Student() {
    return this.StudentActions;
  }
}
