import WelcomePage from "./WelcomePage";
import Login from "./modules/account/views/Login";
import Register from "./modules/account/views/Register";
import { Routes, Route, useNavigate } from "react-router-dom";
import axios from "axios";
import "./App.css";
import Dashboard from "./dashboard";
import StudentDashboard from "./modules/student/views/studentDashboard";
import { ToastContainer } from "react-toastify";
import TeacherDashboard from "./modules/teacher/views/teacherDashboard";
import ParentDashboard from "./modules/parent/views/parentDashboard";
import RegisterTeacher from "./modules/account/views/RegisterTeacher";
import AdminDashboard from "./modules/admin/views/adminDashboard";
import { useEffect } from "react";
import AccountService from "./modules/account/accountService";
import ProfileDetails from "./components/ProfileDetails";

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

  const verifyUserCredits = async () => {
    try {
      await AccountService.Verify();
      navigate("/dashboard");
    } catch (ex: any) {
      localStorage.removeItem("token");
      navigate("/");
    }
  };

  useEffect(() => {
    verifyUserCredits();
  }, []);

  return (
    <>
      <Routes>
        <Route path="/" element={<WelcomePage />} />
        <Route path="/account/profile-details" element={<ProfileDetails />} />
        <Route path="/accounts/login" element={<Login />} />
        <Route path="/accounts/register" element={<Register />} />
        <Route
          path="/accounts/register-teacher"
          element={<RegisterTeacher />}
        />
        <Route path="/dashboard" element={<Dashboard />} />
        <Route path="/dashboard/student" element={<StudentDashboard />} />
        <Route path="/dashboard/teacher" element={<TeacherDashboard />} />
        <Route path="/dashboard/parent" element={<ParentDashboard />} />
        <Route path="/dashboard/admin" element={<AdminDashboard />} />
      </Routes>
      <ToastContainer />
    </>
  );
}

export default App;
