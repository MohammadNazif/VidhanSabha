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

  galleryImages = [
    { url: '/assets/images/4.jpg', title: 'Public Rally & Development Programs' },
    { url: '/assets/images/5.jpg', title: 'Ministerial Meeting in Lucknow' },
    { url: '/assets/images/6.jpg', title: 'Inauguration of New Bridge' },
    { url: '/assets/images/7.jpg', title: 'Connecting with Local Citizens' },
    { url: '/assets/images/13.jpg', title: 'Infrastructure Review Visit' },
    { url: '/assets/images/train.jpg', title: 'Railway Project Discussion' },
    { url: '/assets/images/jal.jpg', title: 'Jal Shakti Mission Review' },
    { url: '/assets/images/dinesh.jpeg', title: 'Public Interaction Session' }
  ];
  activeGalleryIndex = 0;
  galleryInterval: any;

  tickerItems = [
    '63.5 KM Meerut–Hastinapur–Bijnor Rail Line Proposal',
    'Flood Control & Embankment Work in Ganga Belt',
    'Rail Connectivity Push for Hastinapur Tourism',
    'Development of Rural Roads & Bridges',
    'Support for Flood-Affected Farmers',
    'Public Welfare Schemes Implementation in Hastinapur'
  ];

  stats = [
    {
      icon: 'fa-solid fa-train-subway',
      value: '63.5 KM',
      label: 'Meerut–Hastinapur–Bijnor Rail Proposal'
    },
    {
      icon: 'fa-solid fa-water',
      value: 'Flood Control',
      label: 'Ganga Embankment & Protection Works'
    },
    {
      icon: 'fa-solid fa-road',
      value: 'Road Network',
      label: 'Rural Connectivity Development'
    },
    {
      icon: 'fa-solid fa-bridge',
      value: 'Public Works',
      label: 'Bridge & Infrastructure Support'
    }
  ];

  sectors = [
    {
      img: '/assets/images/train.jpg',
      title: 'Meerut–Hastinapur–Bijnor Rail Line',
      desc: 'Dinesh Khatik met Railway Minister Ashwini Vaishnaw and pushed for the proposed 63.5 KM Meerut–Hastinapur–Bijnor railway line to improve tourism, connectivity, and local economic growth in the Hastinapur region.',
      tag: 'Rail Project',
      link: 'https://timesofindia.indiatimes.com/city/meerut/dinesh-khatik-meets-rail-minister-ashwani-vaishnav-urges-him-to-start-hastinapur-bijnor-rail-link/articleshow/97015247.cms'
    },
    {
      img: '/assets/images/jal.jpg',
      title: 'Flood Control & Ganga Protection',
      desc: 'As Minister of State for Jal Shakti and Flood Control, Dinesh Khatik worked on flood protection measures, embankment strengthening, and relief efforts for villages affected by Ganga flooding in Hastinapur areas.',
      tag: 'Flood Control',
      link: 'https://www.amarujala.com/photo-gallery/uttar-pradesh/meerut/hastinapur-minister-of-state-dinesh-khatik-visited-the-flood-affected-area-with-a-bullet?utm_source=chatgpt.com'
    },
    {
      img: '/assets/images/13.jpg',
      title: 'Road & Rural Connectivity',
      desc: 'Focus on improving rural roads, local connectivity, and public infrastructure development across villages and nearby regions of Hastinapur constituency.',
      tag: 'Infrastructure',
      link: 'https://www.thedailyjagran.com/uttar-pradesh/up-govt-approves-rs-10-28-crore-for-hastinapur-pilgrimage-route-widening-details-40050619?utm_source=chatgpt.com'
    }
  ];

  schemes = [
    {
      icon: '💧',
      title: 'Jal Jeevan Mission',
      desc: 'Providing tap water connections to rural households under the Government of India mission.',
      verified: true
    },
    {
      icon: '🏠',
      title: 'PM Awas Yojana',
      desc: 'Affordable housing support for eligible rural and urban families.',
      verified: true
    },
    {
      icon: '🏥',
      title: 'Ayushman Bharat',
      desc: 'Free healthcare coverage up to ₹5 lakh for eligible families.',
      verified: true
    },
    {
      icon: '💰',
      title: 'PM Kisan Samman Nidhi',
      desc: 'Direct financial support provided annually to eligible farmers.',
      verified: true
    },
    {
      icon: '🔥',
      title: 'Ujjwala Yojana',
      desc: 'Free LPG gas connections for economically weaker households.',
      verified: true
    },
    {
      icon: '⚡',
      title: 'Energy Conservation Initiative',
      desc: 'Promotion of energy-saving awareness and efficient electricity usage.',
      verified: true
    }
  ];

  testimonials = [
    {
      name: 'Rakesh Kumar',
      location: 'Hastinapur',
      text: 'The railway proposal can become a major boost for tourism and employment opportunities in our region.'
    },
    {
      name: 'Sunita Devi',
      location: 'Meerut Rural',
      text: 'Flood control work and embankment support have helped many villages during difficult monsoon seasons.'
    },
    {
      name: 'Ankit Sharma',
      location: 'Bijnor',
      text: 'Road connectivity and infrastructure development have improved transportation in nearby rural areas.'
    }
  ];

  navLinks = [
    { label: 'Home', target: 'home' },
    { label: 'Numbers', target: 'stats' },
    { label: 'Development', target: 'development' },
    { label: 'Schemes', target: 'schemes' },
    { label: 'Gallery', target: 'gallery' },
    { label: 'Contact', target: 'contact' }
  ];

  ngOnInit() {
    this.startGalleryAutoPlay();
  }

  ngOnDestroy() {
    if (this.galleryInterval) {
      clearInterval(this.galleryInterval);
    }
  }

  startGalleryAutoPlay() {
    this.galleryInterval = setInterval(() => {
      this.nextGallery();
    }, 5000);
  }

  nextGallery() {
    this.activeGalleryIndex = (this.activeGalleryIndex + 1) % this.galleryImages.length;
  }

  setGalleryIndex(index: number) {
    this.activeGalleryIndex = index;
    clearInterval(this.galleryInterval);
    this.startGalleryAutoPlay();
  }

  getSlotIndex(index: number) {
    const total = this.galleryImages.length;
    return (index - this.activeGalleryIndex + total) % total;
  }

  @HostListener('window:scroll', [])
  onScroll() {
    this.isScrolled = window.scrollY > 60;

    const sections = [
      'home',
      'stats',
      'development',
      'schemes',
      'gallery',
      'testimonials',
      'contact'
    ];

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
      el.scrollIntoView({
        behavior: 'smooth',
        block: 'start'
      });
    }

    this.isMenuOpen = false;
  }

  toggleMenu() {
    this.isMenuOpen = !this.isMenuOpen;
  }
}