export interface Application {
  id: number;
  jobListingId: number;
  jobTitle: string;
  companyName: string;
  userId: number;
  username: string;
  applicationDate: Date;
  status: 'Submitted' | 'ApprovedForInterview' | 'Rejected';
}

export interface CreateApplicationDto {
  jobListingId: number;
}

export interface UpdateApplicationStatusDto {
  status: 'Submitted' | 'ApprovedForInterview' | 'Rejected';
}
