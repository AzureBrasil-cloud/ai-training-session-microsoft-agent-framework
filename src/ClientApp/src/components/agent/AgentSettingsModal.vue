<!-- AgentSettingsModal.vue - Modal de configurações do agente -->
<script setup lang="ts">
import { ref, watch } from 'vue';

interface Props {
  agentName: string;
  agentInstructions: string;
}

const props = defineProps<Props>();

const emit = defineEmits<{
  close: [];
  save: [settings: { name: string; instructions: string }];
}>();

const tempName = ref(props.agentName);
const tempInstructions = ref(props.agentInstructions);

watch(() => props.agentName, (newVal) => {
  tempName.value = newVal;
});

watch(() => props.agentInstructions, (newVal) => {
  tempInstructions.value = newVal;
});

function save() {
  emit('save', {
    name: tempName.value,
    instructions: tempInstructions.value
  });
}
</script>

<template>
  <div class="modal d-block" tabindex="-1" style="background-color: rgba(0,0,0,0.5);">
    <div class="modal-dialog modal-dialog-centered modal-lg">
      <div class="modal-content">
        <div class="modal-header">
          <h5 class="modal-title"><i class="bi bi-gear"></i> Configurações do Agente</h5>
          <button type="button" class="btn-close" @click="emit('close')"></button>
        </div>
        <div class="modal-body">
          <div class="mb-3">
            <label for="agentName" class="form-label">Nome do Agente</label>
            <input
              type="text"
              class="form-control"
              id="agentName"
              v-model="tempName"
              placeholder="Ex: Agente de Cadastro de Carros"
            />
          </div>
          <div class="mb-3">
            <label for="agentInstructions" class="form-label">Instruções do Agente</label>
            <textarea
              class="form-control"
              id="agentInstructions"
              rows="20"
              v-model="tempInstructions"
              placeholder="Descreva como o agente deve se comportar..."
            ></textarea>
          </div>
        </div>
        <div class="modal-footer">
          <button type="button" class="btn btn-neutral" @click="emit('close')">Cancelar</button>
          <button type="button" class="btn btn-primary" @click="save">Salvar</button>
        </div>
      </div>
    </div>
  </div>
</template>

