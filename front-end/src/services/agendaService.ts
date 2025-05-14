import axios from "axios";

const API = axios.create({
  baseURL: import.meta.env.VITE_API_URL,
});

export default {
  listar() {
    return API.get("/").then((res) => res.data);
  },
  criar(item: { name: string; email: string; phone: string }) {
    return API.post("/", item).then((res) => res.data);
  },
  atualizar(item: { id: string; name: string; email: string; phone: string }) {
    return API.put(`/${item.id}`, item).then((res) => res.data);
  },
  remover(id: number) {
    return API.delete(`/${id}`);
  },
};
