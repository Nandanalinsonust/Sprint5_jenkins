export interface Appointment {

  appointmentId: number;

  patientId: number;

  doctorId: number;

  patientName?: string;

  doctorName?: string;

  scheduledDate: string;

  timeSlot: string;

  status: string;

  cancellationReason?: string;

  hasHealthRecord?: boolean;
}

export interface CreateAppointmentRequest {

  doctorId: number;

  scheduledDate: string;

  timeSlot: string;
}

export interface AppointmentStatusUpdateRequest {

  status: string;

  cancellationReason?: string;
}