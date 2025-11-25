import WelcomePage from "./WelcomePage";
import Login from "./modules/account/views/Login";
import Register from "./modules/account/views/Register";
import { Routes, Route, useNavigate } from "react-router-dom";
import axios from "axios";

function App() {
  const navigate = useNavigate();
  axios.defaults.baseURL = "https://localhost:7252/api";
  axios.defaults.headers.common["Content-Type"] = "application/json";

  axios.interceptors.request.use(
    (config) => {
      const token = localStorage.getItem("token");
      if (token) {
        config.headers.Authorization = `Bearer ${token}`;
      }
      return config;
    },
    (error) => Promise.reject(error)
  );

  axios.interceptors.response.use(
    (response) => response,
    (error) => {
      if (error.response?.status === 401) {
        localStorage.removeItem("token");
        navigate("/accounts/login");
      }
      return Promise.reject(error);
    }
  );
  [navigate];

  return (
    <>
      <Routes>
        <Route path="/" element={<WelcomePage />} />
        <Route path="/accounts/login" element={<Login />} />
        <Route path="/accounts/register" element={<Register />} />
      </Routes>
    </>
  );
}

export default App;
