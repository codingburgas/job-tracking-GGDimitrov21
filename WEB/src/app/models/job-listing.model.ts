export interface JobListing {
  id: number;
  title: string;
  company: string;
  description: string;
  publishDate: Date;
  status: 'Active' | 'Inactive';
}

export interface CreateJobListingDto {
  title: string;
  company: string;
  description: string;
}

export interface UpdateJobListingDto {
  title: string;
  company: string;
  description: string;
  status: 'Active' | 'Inactive';
}
