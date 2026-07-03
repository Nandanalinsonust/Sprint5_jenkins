import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

import { Router } from '@angular/router';
import { Doctor } from '../../../../dtos/doctor';
import { DoctorService } from '../../../../services/doctor-service';


@Component({
  selector: 'app-patient-doctors',
  standalone: true,
  imports: [
    CommonModule,
    FormsModule
  ],
  templateUrl: './patient-doctors.html',
  styleUrl: './patient-doctors.css'
})
export class PatientDoctorsComponent implements OnInit {

  doctors: Doctor[] = [];

  filteredDoctors: Doctor[] = [];

  searchTerm = '';

  selectedSpecialisation = '';

  constructor(
    private doctorService: DoctorService,
    private router: Router
  ) {}

  ngOnInit(): void {

    this.doctorService
      .getAll()
      .subscribe({
        next: doctors => {

          this.doctors = doctors;

          this.filteredDoctors = doctors;
        }
      });
  }

  filterDoctors(): void {

    this.filteredDoctors =
      this.doctors.filter(doctor => {

        const matchName =
          !this.searchTerm ||
          doctor.fullName
            .toLowerCase()
            .includes(
              this.searchTerm.toLowerCase()
            );

        const matchSpecialisation =
          !this.selectedSpecialisation ||
          doctor.specialisation.toString() ===
          this.selectedSpecialisation;

        return (
          matchName &&
          matchSpecialisation
        );
      });
  }

  bookDoctor(
    doctorId: number
  ): void {

    this.router.navigate([
      '/patient/book-appointment'
    ],
    {
      queryParams: {
        doctorId
      }
    });
  }
}