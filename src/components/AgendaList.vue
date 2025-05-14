<template>
  <Card class="mb-4">
    <template #title>
      <h2>Agenda de Contatos</h2>
    </template>

    <div class="table-actions mb-2">
      <Button
        label="Novo Contato"
        icon="pi pi-plus"
        class="p-button-sm p-button-primary"
        @click="mostrarForm()"
      />
    </div>

    <DataTable
      :value="itens"
      responsiveLayout="scroll"
      class="p-datatable-sm"
      id="teste"
    >
      <Column field="name" header="Nome" />
      <Column field="phone" header="Telefone" />
      <Column field="email" header="Email" />
      <Column header="Ações" style="width: fit-content; text-align: center">
        <template #body="{ data }">
          <Button
            icon="pi pi-pencil"
            class="p-button-text p-button-sm"
            @click="mostrarForm(data)"
          />
          <Button
            icon="pi pi-trash"
            class="p-button-text p-button-danger p-button-sm"
            @click="remover(data.id)"
          />
        </template>
      </Column>
    </DataTable>
  </Card>

  <AgendaForm
    v-model:visible="formVisivel"
    :item-edit="itemAtual"
    @salvar="carregarItens"
  />
</template>

<script setup lang="ts">
import { ref, onMounted } from "vue";
import AgendaService from "@/services/agendaService";
import AgendaForm from "./AgendaForm.vue";
import { useToast } from "primevue";

interface Item {
  id: string;
  name: string;
  email: string;
  phone: string;
}

const itens = ref<Item[]>([]);
const formVisivel = ref(false);
const itemAtual = ref<Item | null>(null);

const toast = useToast();

async function carregarItens() {
  const lista = await AgendaService.listar();
  itens.value = lista;
  formVisivel.value = false;
}

function mostrarForm(item: Item | null = null) {
  itemAtual.value = item ? { ...item } : null;
  formVisivel.value = true;
}

async function remover(id: number) {
  try {
    if (confirm("Deseja remover este contato?")) {
      await AgendaService.remover(id);
      carregarItens();
    }
    toast.add({
      severity: "success",
      summary: "Sucesso",
      detail: "Contato removido com sucesso.",
      life: 3000,
    });
  } catch (error) {
    console.error("Erro ao remover o contato:", error);
    toast.add({
      severity: "error",
      summary: "Erro",
      detail: "Não foi possível remover o contato.",
      life: 3000,
    });
  }
}

onMounted(carregarItens);
</script>

<style scoped>
.table-actions {
  display: flex;
  justify-content: flex-end;
  margin-bottom: 1rem;
}
</style>
