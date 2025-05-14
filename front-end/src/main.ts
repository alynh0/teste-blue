import "./assets/main.css";
import { createApp } from "vue";
import App from "./App.vue";
import PrimeVue from "primevue/config";

import Lara from "@primevue/themes/lara";
import "primeicons/primeicons.css";
import { ToastService } from "primevue";
import {
  Button,
  Dialog,
  InputText,
  DatePicker,
  DataTable,
  Column,
  InputMask,
} from "primevue";

const app = createApp(App);
app.use(PrimeVue, {
  theme: {
    preset: Lara,
    options: {
      darkModeSelector: false,
    },
  },
});
app.use(ToastService);

app.component("Button", Button);
app.component("Dialog", Dialog);
app.component("InputText", InputText);
app.component("DatePicker", DatePicker);
app.component("DataTable", DataTable);
app.component("Column", Column);
app.component("InputMask", InputMask);

app.mount("#app");
