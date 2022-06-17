
const API_PORT = 5000;

export const environment = {
  production: true,
  ticketPal: {
    api: {
      baseUrl: `http://localhost:${API_PORT}/api`
    }
  }
};
