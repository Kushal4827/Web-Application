import { toast } from "react-toastify";

const toastComponent = (Type, Message, Func) => {
  if (Type === "success") {
    return toast.success(Message, {
      position: "top-center",
      autoClose: 1500,
      hideProgressBar: false,
      closeOnClick: true,
      pauseOnHover: false,
      draggable: false,
      progress: undefined,
      onClose: Func,
    });
  }
  if (Type === "warn") {
    return toast.warn(Message, {
      position: "top-center",
      autoClose: 1500,
      hideProgressBar: false,
      closeOnClick: true,
      pauseOnHover: false,
      draggable: true,
      progress: undefined,
      onClose: Func,
    });
  }
  if (Type === "error") {
    return toast.error(Message, {
      position: "top-center",
      autoClose: 1500,
      hideProgressBar: false,
      closeOnClick: true,
      pauseOnHover: false,
      draggable: true,
      progress: undefined,
      onClose: Func,
    });
  }
  if (Type === "ValidationError") {
    return toast.error(Message, {
      position: "top-right",
      autoClose: 5000,
      hideProgressBar: false,
      closeOnClick: true,
      pauseOnHover: true,
      draggable: true,
      progress: undefined,
      onClose: Func,
    });
  }
};

export default toastComponent;
