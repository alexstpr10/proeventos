import { Component, OnInit, TemplateRef } from '@angular/core';
import { AbstractControl, FormArray, FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { Evento } from '@app/models/Evento';
import { Lote } from '@app/models/Lote';
import { EventoService } from '@app/services/evento.service';
import { LoteService } from '@app/services/lote.service';
import { BsLocaleService } from 'ngx-bootstrap/datepicker';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
import { NgxSpinnerService } from 'ngx-spinner';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-evento-detalhe',
  templateUrl: './evento-detalhe.component.html',
  styleUrls: ['./evento-detalhe.component.scss']
})
export class EventoDetalheComponent implements OnInit {
  modalRef!: BsModalRef;
  evento = {} as Evento;
  loteAtual = {id: 0, nome: '', indice: 0};

  get f() :any{
    return this.form.controls;
  }

  get lotes(): FormArray {
    return this.form.get('lotes') as FormArray;
  }

  get modoEditar(): boolean{
    return this.evento.id > 0;
  }

  form!: FormGroup;

  constructor(private fb: FormBuilder,
              private localeService: BsLocaleService,
              private activatedRoute: ActivatedRoute,
              private eventoService: EventoService,
              private spinner: NgxSpinnerService,
              private modalService: BsModalService,
              private toastr: ToastrService,
              private router: Router,
              private loteService: LoteService){
    this.localeService.use('pt-br');
  }

  public carregarEvento(): void{
    const eventoIdParam = this.activatedRoute.snapshot.paramMap.get('id');

    if(eventoIdParam != null){
      this.spinner.show();
      this.eventoService.getEventoById(+eventoIdParam).subscribe(
        (evento: Evento) => {
          this.evento = {...evento};
          this.form.patchValue(this.evento);
          this.evento.lotes.forEach(lote => {
            this.lotes.push(this.criarLote(lote));
          });
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
      lotes: this.fb.array([])
    });
  }

  public resetForm() : void{
    this.form.reset();
  }

  public cssValidator(campo: AbstractControl | null) : any{
    const ctrl = campo as FormControl;
    return {'is-invalid' : ctrl.errors && ctrl.touched};
  }
  adicionarLote(): void {
    this.lotes.push(this.criarLote({id: 0} as Lote));
  }

  public retornaTituloLote(nome: any): string{
    return nome.value == null || nome.value == '' ? 'Nome do Lote' : nome.value;
  }

  criarLote(lote: Lote): FormGroup {
    return this.fb.group({
      id: [lote.id],
      nome: [lote.nome, Validators.required],
      quantidade: [lote.quantidade, Validators.required],
      preco: [lote.preco, Validators.required],
      dataInicio: [lote.dataInicio],
      dataFim: [lote.dataFim],
    })
  }

  public salvarEvento(): void{
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
          (eventoSalvo: Evento) => {
            this.router.navigate([`./eventos/detalhe/${eventoSalvo.id}`]);
            this.toastr.success('Evento salvo com Sucesso!', 'Sucesso');
          },
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
  public salvarLotes(): void{
    this.spinner.show();
    if(this.form.controls.lotes.valid){
      this.loteService.saveLote(this.evento.id, this.form.value.lotes).subscribe(
        () => {
          this.toastr.success('Lotes salvos com sucesso!', 'Sucesso!');
          //this.lotes.reset();
        },
        (error: any) => {
          this.toastr.error('Erro ao tentar salvar lotes.', 'Erro');
          console.error(error);
        }
      ).add(() => this.spinner.hide());
    }
  }

  public removerLote(template: TemplateRef<any>, indice: number): void{

    this.loteAtual.id = this.lotes.get(indice + '.id')?.value;
    this.loteAtual.nome = this.lotes.get(indice + '.nome')?.value;
    this.loteAtual.indice = indice;

    if(this.loteAtual.id > 0){
      this.modalRef = this.modalService.show(template, {class: 'modal-sm'});
    }
    else{
      this.lotes.removeAt(this.loteAtual.indice);
    }
  }

  public confirmDeleteLote(): void{
    this.modalRef.hide();
    this.spinner.show();

    this.loteService.deleteLote(this.evento.id, this.loteAtual.id).subscribe(
      () => {
        this.toastr.success('Lote deletado com sucesso','Sucesso');
        this.lotes.removeAt(this.loteAtual.indice);
      },
      (error: any) => {
        this.toastr.error(`Erro ao tentar deletar o lote ${this.loteAtual.nome}`, 'Error');
        console.error(error);
      }
    ).add(() => this.spinner.hide());

  }

  public declineDeleteLote(): void{
    this.modalRef.hide();
  }

}
