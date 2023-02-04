
import React from "react";
import TextField from "@mui/material/TextField";
import { Container, Typography } from "@mui/material";
import Grid from "@mui/material/Grid";
import { styled } from "@mui/material/styles";
import Box from "@mui/material/Box";
import Paper from "@mui/material/Paper";

import Button from '@mui/material/Button';
import BuisnesTitleBar from './BuisnesTitleBar'


// import Typography from "@mui/material";

const Item = styled(Paper)(({ theme }) => ({
  backgroundColor: theme.palette.mode === "dark" ? "#1A2027" : "#fff",
  ...theme.typography.body2,
  padding: theme.spacing(1),
  textAlign: "center",
  color: theme.palette.text.secondary,
}));

function ContactInfoForm() {
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

        <BuisnesTitleBar/>








        
         <Box sx={{ '& button': { m: 2 } }}>

<Button variant="contained" size="medium">
Save As Draft
</Button>
<Button variant="contained" size="medium">
Submit
</Button>
</Box>
         
         

          <Grid item xs={12}>
            <Item>
              <TextField
                fullWidth
                required
                id="outlined-required"
                label="Subject"
                type="mail"
                sx={{ marginBottom: "8px" }}
              />
              <TextField
                fullWidth
                required
                id="outlined-required"
                label="Discription"
                type="number"
                sx={{ marginBottom: "8px" }}
              />
              <TextField
                fullWidth
                required
                id="outlined-required"
                label="Purpose"
                sx={{ marginBottom: "8px" }}
              />




        
             
            </Item>


            <Box sx={{ '& button': { m: 2 } }}>

            <Button variant="contained" size="medium">
          Save As Draft
        </Button>
        <Button variant="contained" size="medium">
          Submit
        </Button>
        </Box>

          </Grid>
         
         
        </Box>

      </div>
    </Container>
    

  );
}

export default ContactInfoForm;





