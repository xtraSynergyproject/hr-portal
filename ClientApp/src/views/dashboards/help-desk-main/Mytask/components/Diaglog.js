// ** React Imports
import { useState } from 'react'

// ** MUI Imports
import IconButton from '@mui/material/IconButton'
import Icon from 'src/@core/components/icon'
import Button from '@mui/material/Button'
import Dialog from '@mui/material/Dialog'
import DialogTitle from '@mui/material/DialogTitle'
import FormControl from '@mui/material/FormControl'
import DialogActions from '@mui/material/DialogActions'
import Typography from '@mui/material/Typography'
import Box from '@mui/material/Box'
import Card from '@mui/material/Card'
import TextField from '@mui/material/TextField'
import Grid from '@mui/material/Grid'

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
    <div>
      <Typography variant='text' color='primary' onClick={handleClickOpen}>
       + Add Task
     </Typography>

      <Dialog maxWidth='md' fullWidth open={open} onClose={handleClose}>
        <DialogTitle> Create Task </DialogTitle>
        <IconButton
          aria-label='close'
          onClick={handleClose}
          sx={{ top: 10, right: 10, position: 'absolute', color: 'grey.800' }}
        >
          <Icon icon='mdi:close' />
        </IconButton>
        <Box sx={{ flexGrow: 1, paddingLeft: '25px' }}>
          <Grid container>
            <Grid>
              {/* <DialogActions > Dummy Data </DialogActions> */}
              <Box sx={{ display: 'flex', gap: '5px' }}>
                <Card>
                  <Box sx={{ justifyContent: 'space-between', gap: '4px', width: '600px' }}>
                    <FormControl fullWidth sx={{ display: 'flex', justifyContent: 'space-between' }}>
                      <TextField size='small' />
                    </FormControl>
                  </Box>
                </Card>
                <Button variant='contained' onClick={handleClickOpen}>
                  submit
                </Button>
              </Box>
            </Grid>
          </Grid>
        </Box>
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
