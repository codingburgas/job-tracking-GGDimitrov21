import { Component, Input, OnInit } from '@angular/core';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { Application, UpdateApplicationStatusDto } from '../../../models/application.model';
import { ApplicationService } from '../../../services/application.service';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';

@Component({
  selector: 'app-application-status-modal',
  templateUrl: './application-status-modal.component.html',
  styleUrls: ['./application-status-modal.component.css']
})
export class ApplicationStatusModalComponent implements OnInit {
  @Input() application!: Application; // Input for the application to update
  statusForm!: FormGroup;
  loading = false;
  submitted = false;
  error = '';

  constructor(
    public activeModal: NgbActiveModal,
    private fb: FormBuilder,
    private applicationService: ApplicationService
  ) { }

  ngOnInit(): void {
    this.statusForm = this.fb.group({
      status: [this.application.status, Validators.required]
    });
  }

  get f() { return this.statusForm.controls; }

  onSubmit(): void {
    this.submitted = true;
    this.error = '';

    if (this.statusForm.invalid) {
      return;
    }

    this.loading = true;
    const updateDto: UpdateApplicationStatusDto = {
      status: this.f['status'].value
    };

    this.applicationService.updateApplicationStatus(this.application.id, updateDto).subscribe({
      next: () => {
        this.activeModal.close('updated');
      },
      error: (err) => {
        this.error = 'Failed to update application status.';
        console.error('Update status failed:', err);
        this.loading = false;
      }
    });
  }

  cancel(): void {
    this.activeModal.dismiss('cancel');
  }
}
