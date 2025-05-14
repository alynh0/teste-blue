<template>
  <Dialog header="Adicionar contato" v-model:visible="visible" modal>
    <div class="p-fluid">
      <div class="p-field">
        <label for="name">Nome:</label>
        <InputText class="input-form" id="name" v-model="form.name" />
      </div>
      <div class="p-field">
        <label for="email">E-mail:</label>
        <InputText class="input-form" id="email" v-model="form.email" />
      </div>
      <div class="p-field">
        <label for="phone">Telefone:</label>
        <InputMask
          class="input-form"
          id="phone"
          v-model="form.phone"
          mask="(99) 99999-9999"
          placeholder="(81) 99999-9999"
        />
      </div>
    </div>
    <template #footer>
      <Button label="Salvar" @click="salvar" />
      <Button
        label="Cancelar"
        class="p-button-secondary"
        @click="visible = false"
      />
    </template>
  </Dialog>
</template>

<script setup lang="ts">
import { watch, reactive, computed } from "vue";
import AgendaService from "@/services/agendaService";
import { InputMask } from "primevue";
import { useToast } from "primevue";

interface Contato {
  id?: string;
  name: string;
  email: string;
  phone: string;
}

const props = defineProps<{
  itemEdit: Contato | null;
  visible: boolean;
}>();

const emit = defineEmits<{
  (e: "update:visible", val: boolean): void;
  (e: "salvar"): void;
}>();

const visible = computed({
  get: () => props.visible,
  set: (v) => emit("update:visible", v),
});

const form = reactive<Contato>({
  id: undefined,
  name: "",
  email: "",
  phone: "",
});

const toast = useToast();

watch(
  () => props.itemEdit,
  (val) => {
    if (val) {
      Object.assign(form, val);
    } else {
      form.id = undefined;
      form.name = "";
      form.email = "";
      form.phone = "";
    }
  },
  { immediate: true }
);

async function salvar() {
  try {
    if (form.id != null) {
      await AgendaService.atualizar({
        id: form.id,
        name: form.name,
        email: form.email,
        phone: form.phone,
      });
      toast.add({
        severity: "success",
        summary: "Sucesso",
        detail: "Contato atualizado com sucesso!",
        life: 3000,
      });
    } else {
      await AgendaService.criar({
        name: form.name,
        email: form.email,
        phone: form.phone,
      });
      toast.add({
        severity: "success",
        summary: "Sucesso",
        detail: "Contato criado com sucesso!",
        life: 3000,
      });
    }
  } catch (error) {
    console.error(error);
    toast.add({
      severity: "error",
      summary: "Erro",
      detail: `Erro ao salvar contato: ${error}`,
      life: 3000,
    });
  }
  emit("salvar");
  visible.value = false;
}
</script>

<style scoped>
.input-form {
  width: 100%;
  margin: 14px 0px;
}
</style>
