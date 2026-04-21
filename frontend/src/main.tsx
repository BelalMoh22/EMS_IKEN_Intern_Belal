import { createRoot } from "react-dom/client";
import { ThemeProvider, CssBaseline, IconButton } from "@mui/material";
import { SnackbarProvider, closeSnackbar } from "notistack";
import CloseIcon from "@mui/icons-material/Close";
import theme from "./theme";
import App from "./App";
import "./index.css";

createRoot(document.getElementById("root")!).render(
  <ThemeProvider theme={theme}>
    <CssBaseline />
    <SnackbarProvider
      maxSnack={3}
      anchorOrigin={{ vertical: "bottom", horizontal: "right" }}
      autoHideDuration={4000}
      action={(key) => (
        <IconButton
          size="small"
          onClick={() => closeSnackbar(key)}
          sx={{ color: "white" }}
        >
          <CloseIcon fontSize="small" />
        </IconButton>
      )}
    >
      <App />
    </SnackbarProvider>
  </ThemeProvider>,
);