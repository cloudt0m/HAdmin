import axios from 'axios';

const service = axios.create({
  baseURL: 'https://localhost:5001/api',
  headers: {
    'Content-Type': 'application/json',
  },
  withCredentials: true,
  timeout: 5000,
});

service.interceptors.request.use(
  (config) => {
    return config;
  },
  () => {
    return Promise.reject();
  }
);

service.interceptors.response.use((response) => {
  if (response.status === 200) {
    return response.data;
  } else if (response.status === 204) {
    return;
  } else {
    Promise.reject();
  }
});

export default service;
