import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HealthRecord } from '../../../../dtos/health-record';
import { PatientService } from '../../../../services/patient-service';


@Component({
  selector: 'app-health-records',
  standalone: true,
  imports: [
    CommonModule
  ],
  templateUrl: './health-records.html',
  styleUrl: './health-records.css'
})
export class HealthRecordsComponent implements OnInit {

  records: HealthRecord[] = [];

  constructor(
    private patientService: PatientService
  ) {}

  ngOnInit(): void {

    this.patientService
      .getHealthRecords()
      .subscribe({
        next: data => {
          this.records = data;
        }
      });
  }
}