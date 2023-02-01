import React from "react";
import TextField from "@mui/material/TextField";
import { Container, Typography } from "@mui/material";
import Grid from "@mui/material/Grid";
import { styled } from "@mui/material/styles";
import Box from "@mui/material/Box";
import Paper from "@mui/material/Paper";

// import Typography from "@mui/material";

const Item = styled(Paper)(({ theme }) => ({
  backgroundColor: theme.palette.mode === "dark" ? "#1A2027" : "#fff",
  ...theme.typography.body2,
  padding: theme.spacing(1),
  textAlign: "center",
  color: theme.palette.text.secondary,
}));

function AddressInfoForm() {
  return (
    <Container maxWidth="lg">
      <div>
        <Box
          sx={{
            flexGrow: 1,
            display: "flex",
            flexDirection: "column",
            p: 1,
            "& .MuiTextField-root": { p: 1.2 },
            "& .MuiFormControl-root": { p: 1.2 },
          }}
        >
          {/* <Grid item xs={12} sx={{ marginBottom: "10px", marginTop: "12px" }}>
            <Item>
              <label> */}
            
                {/* <Typography sx={{marginLeft : "3.5%" , marginBottom : "2%"}} component="h3" variant="h6">
                  Address Information
                </Typography> */}

            
              {/* </label>
            </Item>
          </Grid> */}

          <Grid container sx={{marginBottom : "8px"}} spacing={2}>
            <Grid item xs={6}>
              <Item>
                <Typography>Present Address</Typography>

                <TextField
                  fullWidth
                  id="outlined-required"
                  label="Unit Number"
                  sx={{ marginBottom: "8px" }}
                />

                <TextField
                  fullWidth required
                  id="outlined-required"
                  label="Building Number"
                  sx={{ marginBottom: "8px" }}
                />

                <TextField
                  fullWidth required
                  id="outlined-required"
                  label="Street Name"
                  sx={{ marginBottom: "8px" }}
                />

                <TextField
                  fullWidth required
                  id="outlined-required"
                  label="City/Town"
                  sx={{ marginBottom: "8px" }}
                />

                <TextField
                  fullWidth required
                  id="outlined-required"
                  label="Postal Code"
                  sx={{ marginBottom: "8px" }}
                />

                <TextField
                  fullWidth
                  id="outlined-required"
                  label="Additional Number"
                  sx={{ marginBottom: "8px" }}
                />

                <TextField
                  fullWidth
                  id="outlined-required"
                  label="Neighbour Name"
                  sx={{ marginBottom: "8px" }}
                />

                <TextField
                  fullWidth required
                  id="outlined-required"
                  label="Country"
                  sx={{ marginBottom: "8px" }}
                />
              </Item>
            </Grid>

            <Grid item xs={6}>
              <Item>
                <Typography>Permanent Address</Typography>

                <TextField
                  fullWidth
                  id="outlined-required"
                  label="Unit Number"
                  sx={{ marginBottom: "8px" }}
                />

                <TextField
                  fullWidth required
                  id="outlined-required"
                  label="Building Number"
                  sx={{ marginBottom: "8px" }}
                />

                <TextField
                  fullWidth required
                  id="outlined-required"
                  label="Street Name"
                  sx={{ marginBottom: "8px" }}
                />

                <TextField
                  fullWidth required
                  id="outlined-required"
                  label="City/Town"
                  sx={{ marginBottom: "8px" }}
                />

                <TextField
                  fullWidth required
                  id="outlined-required"
                  label="Postal Code"
                  sx={{ marginBottom: "8px" }}
                />

                <TextField
                  fullWidth
                  id="outlined-required"
                  label="Additional Number"
                  sx={{ marginBottom: "8px" }}
                />

                <TextField
                  fullWidth
                  id="outlined-required"
                  label="Neighbour Name"
                  sx={{ marginBottom: "8px" }}
                />

                <TextField
                  fullWidth required
                  id="outlined-required"
                  label="Country"
                  sx={{ marginBottom: "8px" }}
                />
              </Item>
            </Grid>
          </Grid>

        
        </Box>

      </div>
    </Container>

  );
}

export default AddressInfoForm;
