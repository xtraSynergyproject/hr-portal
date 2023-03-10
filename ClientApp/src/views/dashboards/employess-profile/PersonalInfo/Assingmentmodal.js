// // ** React Imports
// import { useState } from 'react'

// // ** MUI Imports
// import Button from '@mui/material/Button'
// import Dialog from '@mui/material/Dialog'


// import DialogTitle from '@mui/material/DialogTitle'
// import DialogContent from '@mui/material/DialogContent'
// import DialogActions from '@mui/material/DialogActions'

// ** Icon Imports


// const DialogCustomized = () => {
//   // ** State
//   const [open, setOpen] = useState(false)
//   const handleClickOpen = () => setOpen(true)
//   const handleClose = () => setOpen(false)

//   return (
//     <div>

//       {/* <Button  onClick={handleClickOpen} sx={{ m: 2 , width: 220 }} variant='contained' >
//     Manage Assignment 
//  </Button> */}

//       <Dialog onClose={handleClose} aria-labelledby='customized-dialog-title' open={open}>
//         <DialogTitle id='customized-dialog-title' sx={{ p: 4 }}>
//           <Typography variant='h6' component='span'>
//             Assignment
//           </Typography>
//           <IconButton
//             aria-label='close'
//             onClick={handleClose}
//             sx={{ top: 10, right: 10, position: 'absolute', color: 'grey.500' }}
//           >
//              <Icon icon='mdi:close' />
//           </IconButton>
//         </DialogTitle>
//         <DialogContent dividers sx={{ p: 4 }}>
//           <Typography gutterBottom>
//        1
//           </Typography>
//           <Typography gutterBottom>
//             2
//           </Typography>
//           <Typography gutterBottom>
//             3
//           </Typography>
//         </DialogContent>
//         <DialogActions  >
//           <Button onClick={handleClose}>Save changes</Button>
//         </DialogActions>
//       </Dialog>
//     </div>
//   )
// }

// export default DialogCustomized



// ** React Imports
import { useState } from 'react'
import * as React from 'react';
import Paper from '@mui/material/Paper';
import Grid from '@mui/material/Grid';
// ** MUI Imports
import IconButton from '@mui/material/IconButton'
import Icon from 'src/@core/components/icon'
import Button from '@mui/material/Button'
import Dialog from '@mui/material/Dialog'
import Select from '@mui/material/Select'
import MenuItem from '@mui/material/MenuItem'
import InputLabel from '@mui/material/InputLabel'
import DialogTitle from '@mui/material/DialogTitle'
import FormControl from '@mui/material/FormControl'
import DialogContent from '@mui/material/DialogContent'
import DialogActions from '@mui/material/DialogActions'
import Typography from '@mui/material/Typography'
import { styled } from '@mui/material/styles';
import AttachFileIcon from '@mui/icons-material/AttachFile';


const Item = styled(Paper)(({ theme }) => ({
  backgroundColor: theme.palette.mode === 'dark' ? '#1A2027' : '#fff',
  ...theme.typography.body2,
  padding: theme.spacing(1),
  textAlign: 'center',
  color: theme.palette.text.secondary,
  widows:300
}));

const SelectWithDialog = () => {
  // ** State
  const [open, setOpen] = useState(false)

  const handleClickOpen = () => {
    setOpen(true)
  }

  const handleClose = () => {
    setOpen(false)
  }

  return (
    <div  sx={{ maxWidth:'100'}}>
      <Typography variant='h6' component='span'>
        <Button variant='contained' onClick={handleClickOpen}>
          Manage Assignment

        </Button>
      </Typography>

      <Dialog maxWidth='md' fullWidth open={open} onClose={handleClose} >
        <DialogTitle>Assignment</DialogTitle>
        <Grid sx={{ display: 'flex' }}>
          <Grid item xs={2} >
            <Button
              sx={{ borderRadius: '90px', flexDirection: 'column' }}
              onClick={handleClose}
              component='label'
            >
              <Icon icon='mdi:close' />
              <p>Close</p>
            </Button>

          </Grid>
          <Grid item xs={2} >

            <Button
              sx={{ borderRadius: '90px', flexDirection: 'column' }}

              component='label'
            >
              <Icon icon='open' />
             <p> N-13-02-2023-14</p>
             
            </Button>

          </Grid>
          <Grid item xs={2} >
            <Button
              sx={{ borderRadius: '90px', flexDirection: 'column' }}

              component='label'
            >
              <Icon icon='open' />
              <p>Draft</p>
              Status
            </Button>

          </Grid>

          <Grid item xs={2} >
            <Button
              sx={{ borderRadius: '90px', flexDirection: 'column' }}
              onClick={handleClose}
              component='label'
            >
              <Icon icon='open' />
             <p > 1</p> 
              
              Version

            </Button>

          </Grid>
          <Grid item xs={2} >
          <Button sx={{ borderRadius: '50px', display: 'flex', flexDirection: 'column' }} component='label'>
                  <Icon icon='mdi:attachment-plus' />
                  <p>Attachment</p>
                  <input type='file' hidden />
                </Button>

          </Grid>
      
          <Grid item xs={2} >
            <Button
              sx={{ borderRadius: '90px', flexDirection: 'column' }}

              component='label'
            >
              <Icon icon='open' />
              Draft<br />
              Status
            </Button>

          </Grid>

        </Grid >
        <IconButton
          aria-label='close'
          onClick={handleClose}
          sx={{ top: 10, right: 10, position: 'absolute', color: 'grey.500' }}
        >
          <Icon icon='mdi:close' />
        </IconButton>
        <DialogContent >
          <form>
            <FormControl sx={{ mr: 20,width: 800, top: 10, }}>
              <InputLabel id='demo-dialog-select-label'>Employee Name * </InputLabel>
              <Select label='Age' labelId='demo-dialog-select-label' id='demo-dialog-select' defaultValue=''>
                <MenuItem value=''>
                  <em>None</em>
                </MenuItem>
                <MenuItem value={10}>rajkumar</MenuItem>
                <MenuItem value={20}>pramod</MenuItem>
                <MenuItem value={30}>arun</MenuItem>
                <MenuItem value={10}>sapna</MenuItem>
                <MenuItem value={20}>sneha</MenuItem>
                <MenuItem value={30}>suraj</MenuItem>
              </Select>
            </FormControl>
           
            <br/>
            <br/>
            
            <FormControl sx={{ mr: 10,width: 800, top: 10, }}>
              <InputLabel id='demo-dialog-select-label'>Career Level</InputLabel>
              <Select label='Age' labelId='demo-dialog-select-label' id='demo-dialog-select' defaultValue=''>
                <MenuItem value=''>
                  <em>None</em>
                </MenuItem>
                <MenuItem value={10}>Ten</MenuItem>
                <MenuItem value={20}>Twenty</MenuItem>
                <MenuItem value={30}>Thirty</MenuItem>
              </Select>
            </FormControl>
            <br/>
            <br/>
            
            <FormControl sx={{ mr: 10,width: 800, top: 10, }}>
              <InputLabel id='demo-dialog-select-label'>Department *</InputLabel>
              <Select label='Age' labelId='demo-dialog-select-label' id='demo-dialog-select' defaultValue=''>
                <MenuItem value=''>
                  <em>None</em>
                </MenuItem>
                <MenuItem value={10}>Ten</MenuItem>
                <MenuItem value={20}>Twenty</MenuItem>
                <MenuItem value={30}>Thirty</MenuItem>
              </Select>
            </FormControl>
            <br/>
            <br/>
             <FormControl sx={{ mr: 10,width: 800, top: 10, }}>
              <InputLabel id='demo-dialog-select-label'>Job *</InputLabel>
              <Select label='Age' labelId='demo-dialog-select-label' id='demo-dialog-select' defaultValue=''>
                <MenuItem value=''>
                  <em>None</em>
                </MenuItem>
                <MenuItem value={10}>Ten</MenuItem>
                <MenuItem value={20}>Twenty</MenuItem>
                <MenuItem value={30}>Thirty</MenuItem>
              </Select>
            </FormControl>
            <br/>
            <br/>
            <FormControl sx={{ mr: 10,width: 800, top: 10, }}>
              <InputLabel id='demo-dialog-select-label'>Position *</InputLabel>
              <Select label='Age' labelId='demo-dialog-select-label' id='demo-dialog-select' defaultValue=''>
                <MenuItem value=''>
                  <em>None</em>
                </MenuItem>
                <MenuItem value={10}>Ten</MenuItem>
                <MenuItem value={20}>Twenty</MenuItem>
                <MenuItem value={30}>Thirty</MenuItem>
              </Select>
            </FormControl>
            <br/>
            <br/>
          <FormControl sx={{ mr: 10,width: 800, top: 10, }}>
              <InputLabel id='demo-dialog-select-label'>Location *</InputLabel>
              <Select label='Age' labelId='demo-dialog-select-label' id='demo-dialog-select' defaultValue=''>
                <MenuItem value=''>
                  <em>None</em>
                </MenuItem>
                <MenuItem value={10}>Ten</MenuItem>
                <MenuItem value={20}>Twenty</MenuItem>
                <MenuItem value={30}>Thirty</MenuItem>
              </Select>
            </FormControl>
            <br/>
            <br/>
            <FormControl sx={{ mr: 10,width: 800, top: 10, }}>
              <InputLabel id='demo-dialog-select-label'>Assignment Grade </InputLabel>
              <Select label='Age' labelId='demo-dialog-select-label' id='demo-dialog-select' defaultValue=''>
                <MenuItem value=''>
                  <em>None</em>
                </MenuItem>
                <MenuItem value={10}>Ten</MenuItem>
                <MenuItem value={20}>Twenty</MenuItem>
                <MenuItem value={30}>Thirty</MenuItem>
              </Select>
            </FormControl>
            <br/>
            <br/>
            <FormControl sx={{ mr: 10,width: 800, top: 10, }}>
              <InputLabel id='demo-dialog-select-label'>Assignment Type</InputLabel>
              <Select label='Age' labelId='demo-dialog-select-label' id='demo-dialog-select' defaultValue=''>
                <MenuItem value=''>
                  <em>None</em>
                </MenuItem>
                <MenuItem value={10}>Ten</MenuItem>
                <MenuItem value={20}>Twenty</MenuItem>
                <MenuItem value={30}>Thirty</MenuItem>
              </Select>
            </FormControl>
            <br/>
            <br/>
            <FormControl sx={{ mr: 10,width: 800, top: 10, }}>
              <InputLabel id='demo-dialog-select-label'>Probation Period</InputLabel>
              <Select label='Age' labelId='demo-dialog-select-label' id='demo-dialog-select' defaultValue=''>
                <MenuItem value=''>
                  <em>None</em>
                </MenuItem>
                <MenuItem value={10}>Ten</MenuItem>
                <MenuItem value={20}>Twenty</MenuItem>
                <MenuItem value={30}>Thirty</MenuItem>
              </Select>
            </FormControl>
            <br/>
            <br/>
            <FormControl sx={{ mr: 10,width: 800, top: 10, }}>
              <InputLabel id='demo-dialog-select-label'>Notice Period</InputLabel>
              <Select label='Age' labelId='demo-dialog-select-label' id='demo-dialog-select' defaultValue=''>
                <MenuItem value=''>
                  <em>None</em>
                </MenuItem>
                <MenuItem value={10}>Ten</MenuItem>
                <MenuItem value={20}>Twenty</MenuItem>
                <MenuItem value={30}>Thirty</MenuItem>
              </Select>
            </FormControl>
            <br/>
            <br/>
            <FormControl sx={{ mr: 10,width: 800, top: 10, }}>
              <InputLabel id='demo-dialog-select-label'>Date Of Join </InputLabel>
              <Select label='Age' labelId='demo-dialog-select-label' id='demo-dialog-select' defaultValue=''>
                <MenuItem value=''>
                  <em>None</em>
                </MenuItem>
                <MenuItem value={10}>Ten</MenuItem>
                <MenuItem value={20}>Twenty</MenuItem>
                <MenuItem value={30}>Thirty</MenuItem>
              </Select>
            </FormControl>
            <br/>
            <br/>
             <FormControl sx={{ mr: 10,width: 800, top: 10, }}>
              <InputLabel id='demo-dialog-select-label'>Assignment Status</InputLabel>
              <Select label='Age' labelId='demo-dialog-select-label' id='demo-dialog-select' defaultValue=''>
                <MenuItem value=''>
                  <em>None</em>
                </MenuItem>
                <MenuItem value={10}>Ten</MenuItem>
                <MenuItem value={20}>Twenty</MenuItem>
                <MenuItem value={30}>Thirty</MenuItem>
              </Select>
            </FormControl>
            <br/>
            <br/>
            <FormControl sx={{ mr: 10,width: 800, top: 10, }}>
              <InputLabel id='demo-dialog-select-label'>Person Full Name</InputLabel>
              <Select label='Age' labelId='demo-dialog-select-label' id='demo-dialog-select' defaultValue=''>
                <MenuItem value=''>
                  <em>None</em>
                </MenuItem>
                <MenuItem value={10}>Ten</MenuItem>
                <MenuItem value={20}>Twenty</MenuItem>
                <MenuItem value={30}>Thirty</MenuItem>
              </Select>
            </FormControl>
          </form>
        </DialogContent>
        <DialogActions>
          <Button onClick={handleClose} variant='outlined'>
            Ok
          </Button>
        </DialogActions>
      </Dialog>
    </div>
  )
}

export default SelectWithDialog














