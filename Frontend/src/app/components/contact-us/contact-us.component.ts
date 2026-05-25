import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';

@Component({
  selector: 'app-contact-us',
  standalone: true,
  imports: [CommonModule, RouterModule],
  templateUrl: './contact-us.component.html',
  styleUrls: ['./contact-us.component.css']
})
export class ContactUsComponent {
  
  onSubmit(event: Event) {
    event.preventDefault();
    // Logic for handling the form submission could go here
    alert('Thank you! Your message has been sent successfully. Our team will contact you shortly.');
  }

}
