import { Component, TemplateRef } from '@angular/core';
import { Router } from '@angular/router';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
import { NgxSpinnerService } from 'ngx-spinner';
import { ToastrService } from 'ngx-toastr';
import { Evento } from 'src/app/models/Evento';
import { EventoService } from 'src/app/services/evento.service';

@Component({
  selector: 'app-evento-lista',
  templateUrl: './evento-lista.component.html',
  styleUrls: ['./evento-lista.component.scss']
})
export class EventoListaComponent {
  modalRef!: BsModalRef;

  public eventos: Evento[] = [];
  public eventosFiltrados: Evento[] = [];
  public exibirImg: boolean = true;
  private _filtroLista: string = "";

  public get filtroLista(): string{
    return this._filtroLista;
  }

  public set filtroLista(value: string)
  {
    this._filtroLista =value;
    this.eventosFiltrados = this.filtroLista ? this.filtrarEventos(this.filtroLista) : this.eventos;
  }

  private filtrarEventos(filtrarPor: string): any {
    filtrarPor = filtrarPor.toLocaleLowerCase();
    return this.eventos.filter(
      (evento: { tema: string; local: string; }) => evento.tema.toLocaleLowerCase().indexOf(filtrarPor) !== -1
      || evento.local.toLocaleLowerCase().indexOf(filtrarPor) !== -1
    )
  }

  constructor(
    private eventoService: EventoService,
    private modalService: BsModalService,
    private toastr: ToastrService,
    private spinner: NgxSpinnerService,
    private router: Router) { }

  public ngOnInit(): void {
    this.spinner.show();
    this.getEventos();
  }

  public alterarImagem(): void {
    this.exibirImg = !this.exibirImg;
  }

  public getEventos(): void{
    this.eventoService.getEventos().subscribe({
      next: (eventosResp: Evento[]) => {
        this.eventos = eventosResp;
        this.eventosFiltrados = this.eventos;
      },
      error: (error: any) => {
        console.log(error);
        this.spinner.hide();
        this.toastr.error('Erro ao carregar os eventos', 'Error!');
      },
      complete: () => {
        this.spinner.hide();
      }
    });
  }

  public openModal(template: TemplateRef<any>){
    this.modalRef = this.modalService.show(template, {class: 'modal-sm'});
  }

  public confirm() : void{
    this.modalRef.hide();
    this.toastr.success('O evento foi excluido com sucesso.', 'Excluido!');
  }

  public decline() : void{
    this.modalRef.hide();
  }

  public detalheEvento(id: number) : void{
    this.router.navigate([`eventos/detalhe/${id}`]);
  }
}
