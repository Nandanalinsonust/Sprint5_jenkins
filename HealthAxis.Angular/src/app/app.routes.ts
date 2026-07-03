import { Routes } from '@angular/router';

import { HomeComponent } from './features/home/home';

import { LoginComponent } from './features/auth/login/login';
import { RegisterComponent } from './features/auth/register/register';

import { PatientLayoutComponent } from './layouts/patient-layout/patient-layout';
import { DoctorLayoutComponent } from './layouts/doctor-layout/doctor-layout';

import { authGuard } from './guards/auth-guard';
import { roleGuard } from './guards/role-guard';

import { PatientDashboardComponent } from './features/patient/dashboard/patient-dashboard/patient-dashboard';
import { PatientProfileComponent } from './features/patient/profile/patient-profile/patient-profile';
import { CompleteProfileComponent } from './features/patient/complete-profile/complete-profile/complete-profile';
import { PatientDoctorsComponent } from './features/patient/doctors/patient-doctors/patient-doctors';
import { PatientAppointmentsComponent } from './features/patient/appointments/patient-appointments/patient-appointments';
import { BookAppointmentComponent } from './features/patient/book-appointment/book-appointment/book-appointment';
import { HealthRecordsComponent } from './features/patient/health-records/health-record/health-records';

import { DoctorDashboardComponent } from './features/doctor/dashboard/doctor-dashboard/doctor-dashboard';
import { DoctorScheduleComponent } from './features/doctor/schedule/doctor-schedule/doctor-schedule';
import { DoctorPatientsComponent } from './features/doctor/patients/doctor-patients/doctor-patients';
import { DoctorRecordsComponent } from './features/doctor/records/doctor-records/doctor-records';
import { DoctorProfileComponent } from './features/doctor/profile/doctor-profile/doctor-profile';
import { firstLoginGuard } from './guards/first-login-guard';
import { ChangePasswordComponent } from './features/doctor/change-password/change-password';


export const routes: Routes = [

  // ======================
  // PUBLIC
  // ======================
  {
    path: '',
    component: HomeComponent
  },
  {
    path: 'login',
    component: LoginComponent
  },
  {
    path: 'register',
    component: RegisterComponent
  },

  // ======================
  // PATIENT FIRST LOGIN ROUTE
  // ======================
  {
    path: 'patient/complete-profile',
    component: CompleteProfileComponent,
    canActivate: [authGuard, roleGuard],
    data: { role: 'Patient' }
  },

  // ======================
  // DOCTOR FIRST LOGIN ROUTE
  // ======================
  {
    path: 'doctor/change-password',
    component: ChangePasswordComponent,
    canActivate: [authGuard, roleGuard],
    data: { role: 'Doctor' }
  },

  // ======================
  // PATIENT AREA
  // ======================
  {
    path: 'patient',
    component: PatientLayoutComponent,
    canActivate: [
      authGuard,
      roleGuard,
      firstLoginGuard
    ],
    data: {
      role: 'Patient'
    },
    children: [
      {
        path: 'dashboard',
        component: PatientDashboardComponent
      },
      {
        path: 'profile',
        component: PatientProfileComponent
      },
      {
        path: 'doctors',
        component: PatientDoctorsComponent
      },
      {
        path: 'appointments',
        component: PatientAppointmentsComponent
      },
      {
        path: 'book-appointment',
        component: BookAppointmentComponent
      },
      {
        path: 'health-records',
        component: HealthRecordsComponent
      },
      {
        path: '',
        redirectTo: 'dashboard',
        pathMatch: 'full'
      }
    ]
  },

  // ======================
  // DOCTOR AREA
  // ======================
  {
    path: 'doctor',
    component: DoctorLayoutComponent,
    canActivate: [
      authGuard,
      roleGuard,
      firstLoginGuard
    ],
    data: {
      role: 'Doctor'
    },
    children: [
      {
        path: 'dashboard',
        component: DoctorDashboardComponent
      },
      {
        path: 'schedule',
        component: DoctorScheduleComponent
      },
      {
        path: 'patients',
        component: DoctorPatientsComponent
      },
      {
        path: 'records',
        component: DoctorRecordsComponent
      },
      {
        path: 'profile',
        component: DoctorProfileComponent
      },
      {
        path: '',
        redirectTo: 'dashboard',
        pathMatch: 'full'
      }
    ]
  },

  // ======================
  // FALLBACK
  // ======================
  {
    path: '**',
    redirectTo: ''
  }
];