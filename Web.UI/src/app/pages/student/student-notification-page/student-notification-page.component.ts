import {Component, OnInit} from '@angular/core';
import {DrawerService} from '../../../components/shared/drawer/drawer.service';
import {StudentNotification} from '../../../core/models/notification.student.model';
import {Utils} from '../../../core/utils/utils';
import {DatePipe} from '@angular/common';
import {StudentsService} from '../../../core/services/students.service';
import {AuthService} from '../../../core/services/auth.service';

@Component({
  selector: 'app-student-notification-page',
  standalone: true,
  imports: [
    DatePipe
  ],
  templateUrl: './student-notification-page.component.html',
  styleUrl: './student-notification-page.component.css'
})
export class StudentNotificationPageComponent implements OnInit {
  notifications: StudentNotification[] = [];
  protected readonly Utils = Utils;

  constructor(private drawerService: DrawerService, private authService: AuthService, private studentService: StudentsService) {
  }


  deleteNotification(id: string) {
    this.notifications = this.notifications.filter(notification => notification.id !== id);
  }

  ngOnInit(): void {
    this.drawerService.SeenAllNotifications();
    this.studentService.GetStudentNotifications(this.authService.GetUserId()).subscribe({
      next: data => {
        this.notifications = data
      }
    })
  }

  onDelete(id: string) {
    this.studentService.DeleteStudentNotifications(this.authService.GetUserId(), id).subscribe(
      {
        next: () => {
          this.deleteNotification(id)
        }
      }
    )
  }
}
