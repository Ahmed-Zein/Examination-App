import {Routes} from '@angular/router';
import {AuthGuard} from './core/guards/auth.guard';
import {AppRoles} from './core/config/configuration';
import {NotFoundPageComponent} from './pages/shared/not-found-page/not-found-page.component';
import {LoginPageComponent} from './pages/auth/login/login-page.component';
import {StudentSubjectsPageComponent} from './pages/student/student-subjects-page/student-subjects-page.component';
import {
  StudentExaminationPageComponent
} from './pages/student/student-examination-page/student-examination-page.component';
import {AdminSubjectsPageComponent} from './pages/admin/admin-subjects-page/admin-subjects-page.component';
import {
  AdminSubjectDetailsPageComponent
} from './pages/admin/admin-subject-details-page/admin-subject-details-page.component';
import {AdminStudentsPageComponent} from './pages/admin/admin-students-page/admin-students-page.component';
import {AdminExamResultsPageComponent} from './pages/admin/admin-exam-results-page/admin-exam-results-page.component';
import {StudentProfilePageComponent} from './pages/student/student-profile-page/student-profile-page.component';
import {RegistrationPageComponent} from './pages/auth/register/registeration-page.component';
import {
  AdminExamManagementPageComponent
} from './pages/admin/admin-exam-management-page/admin-exam-management-page.component';
import {AdminDashboardPageComponent} from './pages/admin/admin-dashboard-page/admin-dashboard-page.component';
import {StudentDashboardPageComponent} from './pages/student/student-dashboard-page/student-dashboard-page.component';
import {
  StudentNotificationPageComponent
} from './pages/student/student-notification-page/student-notification-page.component';

export const routes: Routes = [
  {
    path: 'auth',
    children: [
      {path: 'login', component: LoginPageComponent},
      {path: 'register', component: RegistrationPageComponent},
    ],
  },
  {
    path: 'admin',
    children: [
      {path: 'dashboard', component: AdminDashboardPageComponent},
      {path: 'exams-results', component: AdminExamResultsPageComponent},
      {
        path: 'students',
        children: [
          {path: '', component: AdminStudentsPageComponent},
          {path: ':studentId', component: StudentProfilePageComponent}
        ]
      },
      {
        path: 'subjects',
        children: [
          {path: '', component: AdminSubjectsPageComponent},
          {path: ':subjectId', component: AdminSubjectDetailsPageComponent},
          {path: ':subjectId/exams/:examId', component: AdminExamManagementPageComponent},
        ]
      },
    ],
    canActivate: [AuthGuard],
    data: {roles: [AppRoles.Admin]},
  },
  {
    path: 'student',
    children: [
      {path: 'profile', component: StudentProfilePageComponent},
      {path: 'dashboard', component: StudentDashboardPageComponent},
      {path: 'notification', component: StudentNotificationPageComponent},
      {
        path: 'subjects',
        children: [
          {path: '', component: StudentSubjectsPageComponent},
          {path: ':subjectId/examination', component: StudentExaminationPageComponent},
        ]
      },
    ],
    canActivate: [AuthGuard],
    data: {roles: [AppRoles.Student]},
  },
  {path: 'notfound', component: NotFoundPageComponent},
  {path: '', redirectTo: 'auth/login', pathMatch: 'full'},
  {path: '**', redirectTo: 'notfound', pathMatch: 'full'},
];
