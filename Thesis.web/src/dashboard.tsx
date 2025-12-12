import { useEffect } from "react";
import { useNavigate } from "react-router-dom";
import AccountService from "./modules/account/accountService";

function Dashboard() {
  const navigate = useNavigate();

  useEffect(() => {
    AccountService.GetUserRole().then((res) => {
      switch (res.role) {
        case "Student":
          navigate("/dashboard/student");
          break;
        case "Teacher":
          navigate("/dashboard/teacher");
          break;
        case "Parent":
          navigate("/dashboard/parent");
          break;
        case "Admin":
          navigate("/dashboard/admin");
          break;
        default:
          navigate("/");
          break;
      }
    });
  }, []);

  return null;
}

export default Dashboard;
