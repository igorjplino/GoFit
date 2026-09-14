import { Component } from '@angular/core';
import { RouterLink, RouterLinkActive, RouterOutlet } from '@angular/router';
import { MatIcon } from '@angular/material/icon';

type ProfileNavItem = {
  label: string;
  description: string;
  icon: string;
  route?: string;
}

@Component({
  selector: 'app-profile',
  standalone: true,
  imports: [
    RouterOutlet,
    RouterLink,
    RouterLinkActive,
    MatIcon
  ],
  templateUrl: './profile.component.html',
  styleUrl: './profile.component.scss'
})
export class ProfileComponent {
  navItems: ProfileNavItem[] = [
    {
      label: 'Personal Information',
      description: 'Your name and email',
      icon: 'person',
      route: '/profile/personal-information'
    },
    {
      label: 'Account & Security',
      description: 'Password and sign-in',
      icon: 'lock',
      route: '/profile/account-security'
    },
    {
      label: 'Preferences',
      description: 'Units and display options',
      icon: 'tune'
    },
    {
      label: 'Notifications',
      description: 'Email and push alerts',
      icon: 'notifications'
    }
  ];
}
