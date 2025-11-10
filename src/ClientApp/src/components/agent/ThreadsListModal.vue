<!-- ThreadsListModal.vue - Modal de listagem de threads -->
<script setup lang="ts">
import type { Thread } from '@/models/thread';

interface Props {
  threads: Thread[];
  currentThreadId: string;
}

defineProps<Props>();

const emit = defineEmits<{
  close: [];
  loadThread: [threadId: string];
}>();
</script>

<template>
  <div class="modal d-block" tabindex="-1" style="background-color: rgba(0,0,0,0.5);">
    <div class="modal-dialog modal-dialog-centered modal-lg">
      <div class="modal-content">
        <div class="modal-header">
          <h5 class="modal-title"><i class="bi bi-list-ul"></i> Threads Anteriores</h5>
          <button type="button" class="btn-close" @click="emit('close')"></button>
        </div>
        <div class="modal-body" style="max-height: 60vh; overflow-y: auto;">
          <div v-if="threads.length === 0" class="text-muted text-center py-4">
            Nenhuma thread encontrada.
          </div>
          <div v-else class="table-responsive">
            <table class="table table-hover">
              <thead>
                <tr>
                  <th scope="col" style="width: 40%;">ID da Thread</th>
                  <th scope="col" style="width: 60%;">Primeira Mensagem</th>
                </tr>
              </thead>
              <tbody>
                <tr
                  v-for="thread in threads"
                  :key="thread.id"
                  @click="emit('loadThread', thread.id)"
                  style="cursor: pointer;"
                  :class="{ 'table-active': thread.id === currentThreadId }"
                >
                  <td>
                    <i class="bi bi-chat-dots me-2"></i>
                    <code>{{ thread.id }}</code>
                  </td>
                  <td>
                    <span v-if="thread.firstTruncatedMessage && thread.firstTruncatedMessage.trim()" class="text-muted">
                      {{ thread.firstTruncatedMessage }}
                    </span>
                    <span v-else class="text-muted fst-italic">
                      Sem mensagem
                    </span>
                  </td>
                </tr>
              </tbody>
            </table>
          </div>
        </div>
        <div class="modal-footer">
          <button type="button" class="btn btn-secondary" @click="emit('close')">Fechar</button>
        </div>
      </div>
    </div>
  </div>
</template>

