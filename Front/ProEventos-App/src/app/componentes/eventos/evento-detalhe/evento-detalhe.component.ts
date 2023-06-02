import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute } from '@angular/router';
import { Evento } from '@app/models/Evento';
import { EventoService } from '@app/services/evento.service';
import { BsLocaleService } from 'ngx-bootstrap/datepicker';
import { NgxSpinnerService } from 'ngx-spinner';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-evento-detalhe',
  templateUrl: './evento-detalhe.component.html',
  styleUrls: ['./evento-detalhe.component.scss']
})
export class EventoDetalheComponent implements OnInit {
  evento = {} as Evento;

  get f() :any{
    return this.form.controls;
  }

  form!: FormGroup;

  constructor(private fb: FormBuilder,
              private localeService: BsLocaleService,
              private router: ActivatedRoute,
              private eventoService: EventoService,
              private spinner: NgxSpinnerService,
              private toastr: ToastrService){
    this.localeService.use('pt-br');
  }

  public carregarEvento(): void{
    const eventoIdParam = this.router.snapshot.paramMap.get('id');

    if(eventoIdParam != null){
      this.spinner.show();
      this.eventoService.getEventoById(+eventoIdParam).subscribe(
        (evento: Evento) => {
          this.evento = {...evento};
          this.form.patchValue(this.evento);
        },
        (error: any) => {
          this.spinner.hide();
          this.toastr.error('Erro ao tentar carregar evento.', 'Erro!');
          console.error(error);
        },
        () => this.spinner.hide(),
      );
    }

  }

  ngOnInit(): void{
    this.carregarEvento();
    this.validation();
  }

  public validation(): void{
    this.form = this.fb.group({
      tema: ['', [Validators.required, Validators.minLength(4), Validators.maxLength(50)]],
      local: ['', [Validators.required]],
      dataEvento: ['', Validators.required],
      qtdPessoas: ['', [Validators.required, Validators.max(120000)]],
      imagemURL: ['', Validators.required],
      telefone: ['', Validators.required],
      email: ['', [Validators.required, Validators.email]],
    });
  }

  public resetForm() : void{
    this.form.reset();
  }

  public cssValidator(campo: FormControl) : any{
    return {'is-invalid' : campo.errors && campo.touched};
  }

  public salvarAlteracao(): void{
    this.spinner.show();

    if(this.form.valid){

      if (this.evento.id > 0) {
        this.evento = {id: this.evento.id, ... this.form.value};
        console.log(this.evento);
        this.eventoService.putEvento(this.evento.id, this.evento).subscribe(
          () => this.toastr.success('Evento atualizado com Sucesso!', 'Sucesso'),
          (error: any) => {
            console.error(error);
            this.spinner.hide();
            this.toastr.error('Erro ao atualizar o evento', 'Erro');
          },
          () => this.spinner.hide()
        );
      } else {
        this.evento = { ... this.form.value};
        this.eventoService.postEvento(this.evento).subscribe(
          () => this.toastr.success('Evento salvo com Sucesso!', 'Sucesso'),
          (error: any) => {
            console.error(error);
            this.spinner.hide();
            this.toastr.error('Erro ao salvar o evento', 'Erro');
          },
          () => this.spinner.hide()
        );
      }
    }
  }
}
