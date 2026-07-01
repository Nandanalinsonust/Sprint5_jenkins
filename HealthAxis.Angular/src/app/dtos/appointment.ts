export interface Appointment {
  appointmentId: number;
  patientId: number;
  doctorId: number;
  scheduledDate: string;
  timeSlot: string;
  status: string;
  cancellationReason?: string;
}

export interface CreateAppointmentRequest {
  patientId?: number;
  doctorId: number;
  scheduledDate: string;
  timeSlot: string;
}
