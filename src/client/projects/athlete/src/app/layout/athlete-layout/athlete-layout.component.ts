import { Component } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { HeaderComponent } from '../header/header.component';

@Component({
  selector: 'app-athlete-layout',
  standalone: true,
  imports: [RouterOutlet, HeaderComponent],
  templateUrl: './athlete-layout.component.html',
  styleUrl: './athlete-layout.component.scss'
})
export class AthleteLayoutComponent {

}
