import React from "react";
import TextField from "@mui/material/TextField";
import InputLabel from "@mui/material/InputLabel";
import MenuItem from "@mui/material/MenuItem";
import FormControl from "@mui/material/FormControl";
import Select from "@mui/material/Select";
import { Container, Typography } from "@mui/material";
import Grid from "@mui/material/Grid";
import { styled } from "@mui/material/styles";
import Box from "@mui/material/Box";
import Paper from "@mui/material/Paper";

// import Item from "@mui/material/Item"

const Item = styled(Paper)(({ theme }) => ({
  backgroundColor: theme.palette.mode === "dark" ? "#1A2027" : "#fff",
  ...theme.typography.body2,
  padding: theme.spacing(1),
  textAlign: "center",
  color: theme.palette.text.secondary,
}));

// grid sizing


function PersonalInfoForm() {
  return (
    <Container maxWidth="lg">

        <div>
          <Box
            sx={{
              flexGrow: 1,
              display: "flex",
              flexDirection: "column",
              p: 1,
              '& .MuiTextField-root': { p: 1.2},
              '& .MuiFormControl-root': { p: 1.2}
            }}
          >
            {/* <Grid item xs={12} sx={{ marginBottom: "10px" }}>
              <Item>
                <label> */}
                  {/* <Typography sx={{marginLeft : "3.5%" , marginBottom : "2%"}} component="h3" variant="h6">
                    Personal Information
                  </Typography> */}
                {/* </label>
              </Item>
            </Grid> */}

            <Grid container spacing={2}>
              <Grid item xs={6}>
                <Item>
                  <FormControl fullWidth sx={{ marginBottom: "8px" }}>
                    <InputLabel id="demo-simple-select-label">User</InputLabel>
                    <Select
                      labelId="User-label"
                      id="User"
                
                    >
                      <MenuItem value="Synergy">XtraNet Synergy</MenuItem>
                      <MenuItem value="BPO">XtraNet BPO</MenuItem>
                    </Select>
                  </FormControl>
                  <FormControl fullWidth sx={{ marginBottom: "8px" }}>
                    <InputLabel id="status-label">Person Status</InputLabel>
                    <Select
                      labelId="status-label"
                      id="User-status"
                      
                    >
                      <MenuItem value="active">Active</MenuItem>
                      <MenuItem value="former">Former</MenuItem>
                    </Select>
                  </FormControl>
                  <FormControl fullWidth required sx={{ marginBottom: "8px" }}>
                    <InputLabel id="title-label">Title</InputLabel>
                    <Select
                      labelId="title-label"
                      id="title"
                    
                    >
                      <MenuItem value="Mr.">Mr.</MenuItem>
                      <MenuItem value="Mrs.">Mrs.</MenuItem>
                      <MenuItem value="Ms.">Ms.</MenuItem>
                    </Select>
                  </FormControl>
                  <TextField
                    fullWidth
                    required
                    id="outlined-required"
                    label="First Name"
                    sx={{ marginBottom: "8px" }}
                  />
                  <TextField
                    fullWidth
                    id="outlined-required"
                    label="Middle Name"
                    sx={{ marginBottom: "8px" }}
                  />{" "}
                  <TextField
                    fullWidth
                    required
                    id="outlined-required"
                    label="Last Name"
                    sx={{ marginBottom: "8px" }}
                  />
                  <TextField
                    fullWidth
                    id="outlined-required"
                    label="Person Full Name"
                    sx={{ marginBottom: "8px" }}
                  />{" "}
                  <TextField
                    fullWidth
                    id="outlined-required"
                    label="Full Name (In Arabic)"
                    sx={{ marginBottom: "8px" }}
                  />
                  <FormControl fullWidth required sx={{ marginBottom: "2px" }}>
                    <InputLabel id="Gender-label">Gender</InputLabel>
                    <Select
                      labelId="gender-label"
                      id="gender"
                      //   value={age}
                      //   label="Person Status"
                      //   onChange={handleChange}
                    >
                      <MenuItem value="Male">Male</MenuItem>
                      <MenuItem value="Female">Female</MenuItem>
                      <MenuItem value="Prefer-Not-To-Say">
                        Prefer Not To Say
                      </MenuItem>
                    </Select>
                  </FormControl>
                </Item>
              </Grid>

              <Grid item xs={6}>
                <Item>
                  <TextField
                    fullWidth
                    sx={{ marginBottom: "8px" }}
                    id="date"
                    label="Date Of Birth"
                    type="date"
                    defaultValue="YYYY-MM-DD"
                    InputLabelProps={{
                      shrink: true,
                    }}
                  />

                  <FormControl fullWidth required sx={{ marginBottom: "8px" }}>
                    <InputLabel id="Marital-status-label">
                      Marital Status
                    </InputLabel>
                    <Select
                      labelId="marital"
                      id="marital-status"
                    >
                      <MenuItem value="Married">Married</MenuItem>
                      <MenuItem value="Unmarried">Unmarried</MenuItem>
                      <MenuItem value="Single">Single</MenuItem>
                      <MenuItem value="Divorcee">Divorcee</MenuItem>
                    </Select>
                  </FormControl>

                  <TextField
                    fullWidth
                    sx={{ marginBottom: "8px" }}
                    id="date"
                    label="Date Of Joining"
                    type="date"
                    defaultValue="YYYY-MM-DD"
                    InputLabelProps={{
                      shrink: true,
                    }}
                  />

                  <FormControl fullWidth sx={{ marginBottom: "8px" }}>
                    <InputLabel id="Religion-label">Religion</InputLabel>
                    <Select
                      labelId="Religion"
                      id="religion"
                      //   value={age}
                      //   label="Person Status"
                      //   onChange={handleChange}
                    >
                      <MenuItem value="Hinduism">Hinduism</MenuItem>
                      <MenuItem value="Islam">Islam</MenuItem>
                      <MenuItem value="Sikhism">Sikhism</MenuItem>
                      <MenuItem value="Christanity">Christanity</MenuItem>
                    </Select>
                  </FormControl>

                  <TextField
                    fullWidth
                    sx={{ marginBottom: "8px" }}
                    required
                    id="outlined-required"
                    label="Nationality"
                  />

                  <TextField
                    fullWidth
                    sx={{ marginBottom: "8px" }}
                    id="outlined-required"
                    label="Iqamah No./National ID"
                  />

                  <TextField
                    fullWidth
                    sx={{ marginBottom: "8px" }}
                    id="outlined-required"
                    label="Biometric ID"
                  />

                  <TextField
                    fullWidth
                    id="outlined-required"
                    label="Person No."
                    sx={{ marginBottom: "8px" }}
                  />

                  <TextField
                    fullWidth
                    id="outlined-required"
                    label="Sponsorship No."
                  />
                </Item>
              </Grid>
            </Grid>
          </Box>
        </div>
        
      
    </Container>
  );
}

export default PersonalInfoForm;
