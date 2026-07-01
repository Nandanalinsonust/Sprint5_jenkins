import { Routes } from '@angular/router';
import { Login } from './components/login/login';
import { Register } from './components/register/register';
import { authGuard } from './guards/auth-guard';
import { roleGuard } from './guards/role-guard';
import { PatientDashboard } from './components/patient/patient-dashboard/patient-dashboard';
import { PatientProfileComponent } from './components/patient/patient-profile/patient-profile';
import { PatientAppointmentsComponent } from './components/patient/patient-appointments/patient-appointments';
import { PatientDoctorsComponent } from './components/patient/patient-doctors/patient-doctors';
import { BookAppointmentComponent } from './components/patient/book-appointment/book-appointment';
import { ProfileCompleteComponent } from './components/patient/profile-complete/profile-complete';
import { DoctorDashboardComponent } from './components/doctor/doctor-dashboard/doctor-dashboard';
import { DoctorRecordsComponent } from './components/doctor/doctor-records/doctor-records';
import { FirstLoginComponent } from './components/doctor/first-login/first-login';
import { HealthRecordsComponent } from './components/patient/health-records/health-records';
import { PatientLayoutComponent } from './components/patient/patient-layout/patient-layout';
import { HomeComponent } from './components/home/home';

export const routes: Routes = [
  { path: '', component: HomeComponent },
  { path: 'login', component: Login },
  { path: 'register', component: Register },
  {
    path: 'patient',
    canActivate: [authGuard, roleGuard],
    data: { role: 'Patient' },
    component: PatientLayoutComponent,
    children: [
      { path: 'dashboard', component: PatientDashboard },
      { path: 'profile', component: PatientProfileComponent },
      { path: 'complete-profile', component: ProfileCompleteComponent },
      { path: 'appointments', component: PatientAppointmentsComponent },
      { path: 'doctors', component: PatientDoctorsComponent },
      { path: 'book-appointment', component: BookAppointmentComponent },
      { path: 'health-records', component: HealthRecordsComponent },
      { path: '', redirectTo: 'dashboard', pathMatch: 'full' }
    ]
  },
  {
    path: 'doctor',
    canActivate: [authGuard, roleGuard],
    data: { role: 'Doctor' },
    children: [
      { path: 'dashboard', component: DoctorDashboardComponent },
      { path: 'records', component: DoctorRecordsComponent },
      { path: '', redirectTo: 'dashboard', pathMatch: 'full' }
    ]
  },
  { path: 'doctor/first-login', component: FirstLoginComponent },
  { path: '**', redirectTo: '' }
];
