import { Component, HostListener, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';

import { RouterModule } from '@angular/router';

@Component({
  selector: 'app-landing-page',
  standalone: true,
  imports: [CommonModule, RouterModule],
  templateUrl: './landing-page.component.html',
  styleUrl: './landing-page.component.css'
})
export class LandingPageComponent implements OnInit {
  isScrolled = false;
  activeSection = 'home';
  isMenuOpen = false;

  stats = [
    { icon: '👥', value: '10,00,000+', label: 'Citizens Reached' },
    { icon: '📋', value: '520+', label: 'Projects' },
    { icon: '🏆', value: '45+', label: 'Awards' },
    { icon: '⭐', value: '98%', label: 'Satisfaction' }
  ];

  sectors = [
    {
      img: 'https://images.unsplash.com/photo-1558618666-fcd25c85cd64?w=400&q=80',
      title: 'Rural Road Network',
      desc: '900+ km of new roads connecting every village in the region.',
      tag: 'Infrastructure'
    },
    {
      img: 'https://images.unsplash.com/photo-1454165804606-c3d57bc86b40?w=400&q=80',
      title: 'Jan Sunwai & Resolutions',
      desc: 'Swift action on every citizen complaint through digital platforms.',
      tag: 'Governance'
    },
    {
      img: 'https://images.unsplash.com/photo-1580582932707-520aed937b7b?w=400&q=80',
      title: 'New Schools & Colleges',
      desc: "Empowering girls education as a top priority across all districts.",
      tag: 'Education'
    }
  ];

  schemes = [
    { icon: '🏠', title: 'PM Awas Yojana', desc: 'A pucca home for every family — 10,000+ beneficiaries served.', verified: true },
    { icon: '📚', title: 'Education Mission', desc: 'Smart classrooms and digital laboratories in every government school.', verified: true },
    { icon: '🏥', title: 'Ayushman Bharat', desc: 'Free treatment up to 5 lakh for every eligible family member.', verified: true },
    { icon: '🌾', title: 'Kisan Samman Nidhi', desc: 'Direct financial support to farmers bank accounts every quarter.', verified: true },
    { icon: '💧', title: 'Jal Jeevan Mission', desc: 'Tap water in every home — government clean supply guaranteed.', verified: true },
    { icon: '⚡', title: 'Ujjwala Yojana', desc: 'LPG connections and uninterrupted power supply for every household.', verified: true }
  ];

  testimonials = [
    { name: 'Ramesh Mahiwar', location: 'Village Resident', text: 'Khatik ji ne hamare gaon ki sadak banwayi. Unka kaam sach mein qaabil-e-taareef hai.' },
    { name: 'Sunita Devi', location: 'Beneficiary', text: 'PM Awas ke tahat mujhe ghar mila. Dil se shukriya is madad ke liye.' },
    { name: 'Ankit Sharma', location: 'Youth Participant', text: 'Skill training program se mujhe nayi raah mili. Ek poora naya safar shuru hua.' }
  ];

  navLinks = [
    { label: 'Home', target: 'home' },
    { label: 'Development', target: 'development' },
    { label: 'Schemes', target: 'schemes' },
    { label: 'Gallery', target: 'gallery' },
    { label: 'Contact', target: 'contact' }
  ];

  ngOnInit() { }

  @HostListener('window:scroll', [])
  onScroll() {
    this.isScrolled = window.scrollY > 60;
    const sections = ['home', 'stats', 'development', 'schemes', 'gallery', 'testimonials', 'contact'];
    for (const sec of [...sections].reverse()) {
      const el = document.getElementById(sec);
      if (el && window.scrollY >= el.offsetTop - 120) {
        this.activeSection = sec;
        break;
      }
    }
  }

  scrollTo(sectionId: string) {
    const el = document.getElementById(sectionId);
    if (el) {
      el.scrollIntoView({ behavior: 'smooth', block: 'start' });
    }
    this.isMenuOpen = false;
  }

  toggleMenu() {
    this.isMenuOpen = !this.isMenuOpen;
  }
}
