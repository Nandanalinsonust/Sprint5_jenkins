import { CommonModule } from '@angular/common';
import { Component, inject, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { DoctorPortalService } from '../../../services/doctor-portal-service';
import { HealthRecord } from '../../../dtos/health-record';

@Component({
  selector: 'app-doctor-records',
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './doctor-records.html',
  styleUrl: './doctor-records.css',
})
export class DoctorRecordsComponent implements OnInit {
  doctorPortalService = inject(DoctorPortalService);
  fb = inject(FormBuilder);
  records: HealthRecord[] = [];
  form!: FormGroup;

  ngOnInit(): void {
    this.form = this.fb.group({
      patientId: [0, Validators.required],
      doctorId: [0],
      appointmentId: [0, Validators.required],
      diagnosis: ['', Validators.required],
      prescription: ['', Validators.required],
      notes: ['']
    });

    this.doctorPortalService.getHealthRecords().subscribe({ next: data => this.records = data });
  }

  saveRecord(): void {
    if (this.form.valid) {
      this.doctorPortalService.addHealthRecord(this.form.value).subscribe({ next: () => this.form.reset() });
    }
  }
}
