export interface HealthRecord {
  healthRecordId: number;
  patientId: number;
  doctorId: number;
  appointmentId: number;
  visitDate: string;
  diagnosis: string;
  prescription: string;
  notes?: string;
}

export interface CreateHealthRecordRequest {
  patientId: number;
  doctorId: number;
  appointmentId: number;
  diagnosis: string;
  prescription: string;
  notes?: string;
}
