



// ** React Imports
import { useState } from 'react'

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
    <div  >
     <Typography variant='h6' component='span'>
        <Button variant='contained' onClick={handleClickOpen}>
          Manage Contract    
        </Button>
      </Typography>
   
      <Dialog maxWidth='md' fullWidth open={open} onClose={handleClose} >
        <DialogTitle>Contract</DialogTitle>
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














