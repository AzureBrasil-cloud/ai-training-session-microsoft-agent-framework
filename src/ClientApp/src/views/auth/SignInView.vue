<script setup lang="ts">
import {ADM_CREDENTIALS} from '@/constants/admCredentials';
import {validators} from '@/utils/validators';
import {onMounted, ref} from 'vue';
import {useRouter} from 'vue-router';
import HelpButton from "@/components/common/HelpButton.vue";
import AzureBrasilSignIn from '@/components/common/AzureBrasilSignIn.vue';

const router = useRouter();

let email = ref("");
let password = ref("");

let emailError = ref("");
let passwordError = ref("");


const darkTheme = () => {
  document.documentElement.setAttribute('data-bs-theme', 'dark');
  sessionStorage.setItem('theme', 'dark');
}

const handleSubmit = async () => {
  try {
    clearErrors();
    if (email.value === "") {
      emailError.value = "Email é um campo obrigatório.";
    }

    if (password.value === "") {
      passwordError.value = "Senha é um campo obrigatório.";
    }

    if (email.value === "" || password.value === "") {
      return;
    }

    if (email.value === ADM_CREDENTIALS.email && password.value === ADM_CREDENTIALS.password) {
      darkTheme();
      sessionStorage.setItem("loggedUser", JSON.stringify({
        role: "admin",
        email: ADM_CREDENTIALS.email
      }));
      router.push({name: "home"});
    } else {
      if (!validators.isValidEmail(email.value)) {
        emailError.value = "Email inválido.";
      }

      if (!validators.isValidPassword(password.value)) {
        passwordError.value = "A senha precisa ter pelo menos 8 caracteres.";
      }

      if (hasErrors()) {
        return;
      }

      darkTheme();
      sessionStorage.setItem("loggedUser", JSON.stringify({
        role: "user",
        email: email.value
      }));
      router.push({name: "home"});
    }
  } catch (error) {
    console.error("Erro ao fazer login:", error);
  }
};

const hasErrors = () => {
  return emailError.value || passwordError.value;
};


const handleChangePassword = () => {
  if (passwordError && password.value.length >= 8) {
    passwordError.value = "";
  }
};

const handleChangeEmail = () => {
  if (emailError && email.value.length >= 8) {
    emailError.value = "";
  }
}

const clearErrors = () => {
  emailError.value = "";
  passwordError.value = "";
};

const videoUrl = `${window.location.origin}/videos/login.mp4`;
const logo = `${window.location.origin}/images/Logo.svg`;

const canvasRef = ref<HTMLCanvasElement | null>(null);

onMounted(() => {
  const canvas = canvasRef.value;
  if (!canvas) return;

  const ctx = canvas.getContext('2d');
  if (!ctx) return;

  let { width, height } = canvas.getBoundingClientRect();
  canvas.width = width;
  canvas.height = height;

  const resizeObserver = new ResizeObserver(entries => {
    for (let entry of entries) {
      const { width: newWidth, height: newHeight } = entry.contentRect;
      canvas.width = newWidth;
      canvas.height = newHeight;
      width = newWidth;
      height = newHeight;
    }
  });

  resizeObserver.observe(canvas);


  const orbs: Orb[] = [];
  const orbCount = 40;

  class Orb {
    x: number;
    y: number;
    maxSize: number;
    speed: number;
    speedX: number;
    speedY: number;
    life: number;

    constructor() {
      this.x = 0;
      this.y = 0;
      this.maxSize = 0;
      this.speed = 0;
      this.speedX = 0;
      this.speedY = 0;
      this.life = Math.random() * Math.PI * 2;
      this.reset();
    }

    reset() {
      if (!canvas) return;
      this.x = Math.random() * width;
      this.y = Math.random() * height;
      this.maxSize = Math.random() * 50 + 20;
      this.speed = Math.random() * 0.01 + 0.005;
      this.speedX = (Math.random() - 0.5) * 0.3;
      this.speedY = (Math.random() - 0.5) * 0.3;
    }

    update() {
      if (!canvas) return;
      this.life += this.speed;
      this.x += this.speedX;
      this.y += this.speedY;

      if (this.x < -this.maxSize) this.x = width + this.maxSize;
      if (this.x > width + this.maxSize) this.x = -this.maxSize;
      if (this.y < -this.maxSize) this.y = height + this.maxSize;
      if (this.y > height + this.maxSize) this.y = -this.maxSize;
    }

    draw() {
      if (!ctx) return;
      const pulse = Math.sin(this.life) * 0.5 + 0.5;
      const size = this.maxSize * pulse;
      const opacity = pulse * 0.35;

      const gradient = ctx.createRadialGradient(this.x, this.y, 0, this.x, this.y, size);
      gradient.addColorStop(0, `rgba(158, 168, 255, ${opacity * 0.6})`);
      gradient.addColorStop(0.4, `rgba(158, 168, 255, ${opacity * 0.3})`);
      gradient.addColorStop(0.7, `rgba(158, 168, 255, ${opacity * 0.1})`);
      gradient.addColorStop(1, `rgba(158, 168, 255, 0)`);

      ctx.fillStyle = gradient;
      ctx.beginPath();
      ctx.arc(this.x, this.y, size, 0, Math.PI * 2);
      ctx.fill();
    }
  }

  for (let i = 0; i < orbCount; i++) {
    orbs.push(new Orb());
  }

  let time = 0;

  function animate() {
    if (!canvas || !ctx) return;
    time += 0.002;

    const angle = time;
    const x1 = width * 0.5 + Math.cos(angle) * width * 0.5;
    const y1 = height * 0.5 + Math.sin(angle) * height * 0.5;
    const x2 = width * 0.5 + Math.cos(angle + Math.PI) * width * 0.5;
    const y2 = height * 0.5 + Math.sin(angle + Math.PI) * height * 0.5;

    const gradient = ctx.createLinearGradient(x1, y1, x2, y2);

    gradient.addColorStop(0, `#1F1F1F`);
    gradient.addColorStop(1, `#323232`);

    ctx.fillStyle = gradient;
    ctx.fillRect(0, 0, width, height);

    orbs.forEach(orb => {
      orb.update();
      orb.draw();
    });

    requestAnimationFrame(animate);
  }

  animate();
});
</script>

<template>
  <HelpButton>
    <div class="d-flex justify-content-center my-4">
      <video
        ref="player"
        :src="videoUrl"
        controls
        loop
        autoplay
        muted
        playsinline
        style="width: 100%;"
      ></video>
    </div>
    <h2 class="mb-3"><i class="bi bi-person-check-fill px-2"></i> Descritivo da Página de Login</h2>
    <p>
      A página de <strong>Login</strong> permite que os usuários acessem o sistema informando suas
      credenciais (email e senha). Ela fornece autenticação simulada com base em regras definidas no
      frontend, sem conexão com backends reais ou bancos de dados persistentes.
    </p>

    <p>
      <h5 class="mt-6 mb-3 bg-gray-100 p-2 rounded"><i class="bi bi-list-task px-2"></i>
        Funcionalidades</h5>
      <ul>
        <li><strong>Autenticação de usuários comuns:</strong> Qualquer email válido e senha com no
          mínimo 8 caracteres permite o login como <code>usuário comum</code>.
        </li>
        <li><strong>Autenticação de administrador:</strong> O acesso como <code>administrador</code>
          é
          feito usando:
          <ul>
            <li><strong>Email:</strong> <code>adm@adm.com</code></li>
            <li><strong>Senha:</strong> <code>adm</code></li>
          </ul>
        </li>
        <li><strong>Validação de campos:</strong> O sistema realiza validações locais para garantir
          que o email seja válido e a senha tenha o comprimento mínimo exigido.
        </li>
        <li><strong>Feedback imediato:</strong> Erros de validação são exibidos diretamente abaixo
          dos
          campos de entrada.
        </li>
        <li><strong>Redirecionamento automático:</strong> Após um login bem-sucedido, o usuário é
          redirecionado para a rota <code>home</code>.
        </li>
      </ul>
    </p>

    <p>
      <h5 class="mt-6 mb-3 bg-gray-100 p-2 rounded bck-h"><i class="bi bi-asterisk px-2"></i>Processo Técnico</h5>
      O sistema realiza a autenticação diretamente no frontend. As credenciais de administrador
      estão armazenadas como constante no código-fonte (em <code>ADM_CREDENTIALS</code>), enquanto
      as demais validações são feitas com funções auxiliares presentes no arquivo <code>validators.ts</code>.
    </p>

    <p>
      Ao realizar um login com sucesso, os dados do usuário (email e tipo de acesso) são salvos no
      <strong>sessionStorage</strong> com a chave <code>loggedUser</code>, possibilitando o uso da
      sessão em outras páginas do sistema.
    </p>

    <p>
    <h5 class="mt-6 mb-3 bg-gray-100 p-2 rounded bck-h"><i class="bi bi-check-circle px-2"></i> Regras de Validação</h5>
    <ul>
      <li><strong>Email:</strong> Deve ter um formato válido (ex: <code>usuario@email.com</code>).
      </li>
      <li><strong>Senha:</strong> Deve ter no mínimo <strong>8 caracteres</strong>, exceto no caso
        do administrador (<code>adm</code>).
      </li>
    </ul>
    </p>

    <p>
      <h5 class="mt-6 mb-3 bg-gray-100 p-2 rounded bck-h"><i class="bi bi-bullseye px-2"></i>
        Objetivo</h5>
      Esta funcionalidade foi criada para simular o processo de login em um ambiente controlado,
      útil para protótipos, testes ou demonstrações sem dependência de autenticação real.
    </p>

    <p>
      <h5 class="mt-6 mb-3 bg-gray-100 p-2 rounded bck-h"><i class="bi bi-link-45deg px-2"></i>
        Links
        Úteis</h5>
      <ul>
        <li>
          <a href="https://developer.mozilla.org/pt-BR/docs/Web/API/Window/sessionStorage"
             target="_blank" rel="noopener">
            Documentação – sessionStorage
          </a>
        </li>
        <li>
          <a href="https://vuejs.org/guide/essentials/reactivity-fundamentals.html" target="_blank"
             rel="noopener">
            Vue 3 – Fundamentos de Reatividade
          </a>
        </li>
      </ul>
    </p>
  </HelpButton>

  <div
    class="position-relative d-flex justify-content-center px-5 py-5 p-lg-0 w-100 overflow-hidden"
    data-x-type="page" id="bg-conta">
    <canvas ref="canvasRef" style="position: absolute; top: 0; left: 0; width: 100%; height: 100%; z-index: 0;"></canvas>
    <div
      id="back"
      class="col-lg-5 col-xl-5 p-12 p-xl-10 position-absolute start-0 top-0 min-vh-100 overflow-y-hidden d-none d-lg-flex flex-column border-end-lg" style="position: relative; z-index: 1;">
      <div class="d-flex flex-column flex-grow-1">
        <div class="d-flex justify-content-center align-items-center flex-grow-1" id="logo">
          <a class="d-block" href="#">
            <img :src="logo" width="500" alt="..." class="logo-animation"/>
          </a>
        </div>

        <div class="mt-auto mb-8 w-lg-75">
          <h1 class="ls-tight mb-4 text-white">
            Tecnologia que move você!
          </h1>
          <p class="pe-lg-10 text-white">
            Experiência inteligente para compra e venda de veículos e peças.
          </p>
        </div>
      </div>
    </div>
    <div
      class="col-12 col-md-9 col-lg-7 offset-xl-7 offset-lg-5 vh-lg-100 d-flex flex-column justify-content-center py-lg-16 px-lg-20 position-relative" style="position: relative; z-index: 1;">
      <div class="row">
        <div class="col-lg-10 col-md-9 col-xl-8 col-xxl-7 mx-auto ms-xl-0 rounded-2 p-8" id="bg-vidro">
          <div class="mb-12">
            <h1 class="h2 ls-tight fw-bolder text-white">
              Entre na sua conta
            </h1>
            <p class="text-sm mt-2 text-body-secondary">
              Insira suas credenciais abaixo.
            </p>
          </div>

          <form class="vstack gap-5">
            <div>
              <label class="form-label" for="email">Email</label>
              <input v-model="email" type="email"
                     :class="['form-control', emailError ? 'is-invalid' : '']" id="email"
                     placeholder="Seu email"
                     @input="handleChangeEmail">
              <span v-if="emailError" class="mt-2 invalid-feedback">{{ emailError }}</span>
            </div>
            <div>
              <div class="d-flex align-items-center mb-2">
                <label class="form-label mb-0" for="password">Senha</label>
              </div>
              <input v-model="password" type="password"
                     :class="['form-control', passwordError ? 'is-invalid' : '']" id="password"
                     placeholder="Sua senha"
                     autocomplete="current-password"
                     @input="handleChangePassword">
              <span v-if="passwordError" class="mt-2 invalid-feedback">{{ passwordError }}</span>
            </div>
            <div>
              <button @click.prevent="handleSubmit" class="btn btn-neutral w-100">
                Entrar
              </button>
            </div>
            <AzureBrasilSignIn />
          </form>
        </div>
      </div>
    </div>
  </div>

</template>

<style scoped>
@keyframes slideInFromLeft {
  0% {
    transform: translateX(-100%);
    opacity: 0;
  }
  100% {
    transform: translateX(0);
    opacity: 1;
  }
}

.logo-animation {
  animation: 1s ease-out 0s 1 slideInFromLeft;
}
</style>

