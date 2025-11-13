import { createRouter, createWebHistory } from 'vue-router'
import HomeView from '../views/HomeView.vue'
import SignInView from '@/views/auth/SignInView.vue'

const isAuthenticated = () => {
  const loggedUser = sessionStorage.getItem("loggedUser");

  if (loggedUser) {
    const user = JSON.parse(loggedUser);
    return user.hasOwnProperty("email") && user.hasOwnProperty("role");
  }
}

const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),
  linkActiveClass: 'active',
  routes: [
    {
      path: '/',
      name: 'home',
      component: HomeView,
      meta: { requiresAuth: true }
    },
    {
      path: '/orders',
      name: 'orders',
      component: () => import('../views/orders/OrdersView.vue'),
      meta: { requiresAuth: true }
    },
    {
      path: "/sign-in",
      name: "signin",
      component: SignInView
    },
    {
      path: '/info-chat',
      name: 'info-chat',
      component: () => import('../views/infoChat/infoChatView.vue'),
      meta: { requiresAuth: true }
    },
    {
      path: '/orders-chat',
      name: 'orders-chat',
      component: () => import('../views/ordersChat/ordersChatView.vue'),
      meta: { requiresAuth: true }
    },
    {
      path: '/car-registration',
      name: 'car-registration',
      component: () => import('../views/carRegistration/carRegistrationView.vue'),
      meta: { requiresAuth: true }
    },
    {
      path: '/customer-policies',
      name: 'customer-policies',
      component: () => import('../views/customerPolicies/customerPoliciesView.vue'),
      meta: { requiresAuth: true }
    },
    {
      path: '/car-parts-price',
      name: 'car-parts-price',
      component: () => import('../views/carParts/CarPartPriceView.vue'),
      meta: { requiresAuth: true }
    },
    {
      path: '/car-parts-stock',
      name: 'car-parts-stock',
      component: () => import('../views/carParts/CarPartStockView.vue'),
      meta: { requiresAuth: true }
    },
    {
      path: '/car-parts',
      name: 'car-parts',
      component: () => import('../views/carParts/CarPartsView.vue'),
      meta: { requiresAuth: true }
    },
     {
      path: '/car-parts-product',
      name: 'car-parts-product',
      component: () => import('../views/carParts/CarPartProductView.vue'),
      meta: { requiresAuth: true }
    },
      {
      path: '/discounts-client',
      name: 'discounts-client',
      component: () => import('../views/discounts/DiscountClientView.vue'),
      meta: { requiresAuth: true }
    },
      {
      path: '/discounts-manager',
      name: 'discounts-manager',
      component: () => import('../views/discounts/DiscountManagerView.vue'),
      meta: { requiresAuth: true }
    },
    {
      path: '/send-image-pre-order',
      name: 'send-image-pre-order',
      component: () => import('../views/orders/sendImagePreOrderView.vue'),
      meta: { requiresAuth: true }
    },
    {
      path: '/image-pre-orders',
      name: 'image-pre-orders',
      component: () => import('../views/orders/imagePreOrderView.vue'),
      meta: { requiresAuth: true }
    },
    {
      path: '/audio-pre-orders',
      name: 'audio-pre-orders',
      component: () => import('../views/orders/audioPreOrderView.vue'),
      meta: { requiresAuth: true }
    },
    {
      path: '/send-audio-pre-order',
      name: 'send-audio-pre-order',
      component: () => import('../views/orders/sendAudioPreOrderView.vue'),
      meta: { requiresAuth: true }
    },
    {
      path: '/reviews',
      name: 'reviews',
      component: () => import('../views/reviews/reviews.vue'),
      meta: { requiresAuth: true }
    },
    {
      path: '/send-review',
      name: 'send-review',
      component: () => import('../views/reviews/sendReview.vue'),
      meta: { requiresAuth: true }
    },
    {
      path: '/feedback-classifier',
      name: 'feedback-classifier',
      component: () => import('../views/feedbackClassifier/FeedbackClassifierView.vue'),
      meta: { requiresAuth: true }
    },
    {
      path: '/car-sales',
      name: 'car-sales',
      component: () => import('../views/carSales/CarSalesListView.vue'),
      meta: { requiresAuth: true }
    }
  ],
})

router.beforeEach((to, from, next) => {
  const requiresAuth = to.matched.some(record => record.meta.requiresAuth);
  const userIsAuthenticated = isAuthenticated();

  if (requiresAuth && !userIsAuthenticated) {
    // Se a rota requer autenticação e o usuário não está logado,
    // redireciona para a página de sign-in.
    next({ name: 'signin' });
  } else if (to.name === 'signin' && userIsAuthenticated) {
    // Opcional: se o usuário já está logado e tenta acessar a página de sign-in,
    // redireciona para a página inicial.
    next({ name: 'home' });
  }
  else {
    // Caso contrário, permite a navegação.
    next();
  }
});

export default router
