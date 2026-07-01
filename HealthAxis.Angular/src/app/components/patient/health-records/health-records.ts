import { CommonModule } from '@angular/common';
import { Component, inject, OnInit } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { HealthRecord } from '../../../dtos/health-record';
import { PatientService } from '../../../services/patient-service';

@Component({
  selector: 'app-health-records',
  imports: [CommonModule, FormsModule],
  templateUrl: './health-records.html',
  styleUrl: './health-records.css',
})
export class HealthRecordsComponent implements OnInit {
  private patientService = inject(PatientService);
  records: HealthRecord[] = [];
  filteredRecords: HealthRecord[] = [];
  searchTerm = '';
  page = 1;
  pageSize = 4;

  ngOnInit(): void {
    this.patientService.getHealthRecords().subscribe({
      next: data => {
        this.records = data;
        this.filteredRecords = data;
      }
    });
  }

  filterRecords(): void {
    const term = this.searchTerm.toLowerCase();
    this.filteredRecords = this.records.filter(record =>
      !term || record.diagnosis.toLowerCase().includes(term) || record.prescription.toLowerCase().includes(term)
    );
    this.page = 1;
  }

  get pagedRecords(): HealthRecord[] {
    const start = (this.page - 1) * this.pageSize;
    return this.filteredRecords.slice(start, start + this.pageSize);
  }

  totalPages(): number {
    return Math.max(1, Math.ceil(this.filteredRecords.length / this.pageSize));
  }
}
