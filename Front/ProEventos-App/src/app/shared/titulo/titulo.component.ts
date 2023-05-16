import { Component, Input, OnInit } from '@angular/core';
import { Router } from '@angular/router';

@Component({
  selector: 'app-titulo',
  templateUrl: './titulo.component.html',
  styleUrls: ['./titulo.component.scss']
})
export class TituloComponent implements OnInit {
  @Input() titulo: string = "";
  @Input() subtitulo = 'Desde 2023';
  @Input() iconClass = 'fa fa-user';
  @Input() botaolistar = false;
  constructor(private router: Router) { }

  ngOnInit() {
  }

  listar(): void {
    this.router.navigate([`/${this.titulo.toLowerCase()}/lista`])
  }

}
