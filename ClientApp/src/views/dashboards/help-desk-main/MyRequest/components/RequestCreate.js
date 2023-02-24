// ** React Imports
import { useState } from 'react'

// ** MUI Imports
import IconButton from '@mui/material/IconButton'
import Icon from 'src/@core/components/icon'
import Button from '@mui/material/Button'
import Dialog from '@mui/material/Dialog'
import DialogTitle from '@mui/material/DialogTitle'
import DialogActions from '@mui/material/DialogActions'
import Typography from '@mui/material/Typography'
import Box from '@mui/material/Box'
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
      <Typography variant='h6' component='span'>
        <Button variant='contained' onClick={handleClickOpen}>
          Request
        </Button>
      </Typography>

      <Dialog maxWidth='md' fullWidth open={open} onClose={handleClose}>
        <DialogTitle>My Request Home</DialogTitle>
        <IconButton
          aria-label='close'
          onClick={handleClose}
          sx={{ top: 10, right: 10, position: 'absolute', color: 'grey.800' }}
        >
          <Icon icon='mdi:close' />
        </IconButton>
        <Box sx={{ flexGrow: 1 }}>
          <Grid container>
            <Grid>
              <DialogActions>
                {' '}
                <img
                  src='https://media.istockphoto.com/id/1155292695/photo/written-word-submit-request-on-blue-keyboard-button-online-submission-concept.jpg?s=612x612&w=0&k=20&c=0PS9XQyBoJS6X0gEA800BlPxCQwzJOFsmVHOdAMLI2s='
                  xs={{ with: '300' }}
                ></img>
              </DialogActions>
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
