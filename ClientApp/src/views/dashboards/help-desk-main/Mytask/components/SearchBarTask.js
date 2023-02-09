// ** React Imports
import { useState } from 'react'

// ** MUI Imports
import Box from '@mui/material/Box'
import Card from '@mui/material/Card'
import TextField from '@mui/material/TextField'
import FormControl from '@mui/material/FormControl'
import Button from 'src/@core/theme/overrides/button'
// ** Icon Imports
import Icon from 'src/@core/components/icon'

// ** Hooks Imports
import useBgColor from 'src/@core/hooks/useBgColor'

const RequestSearchBar = () => {
  // ** States
  const [cvc1, setCvc1] = useState('')
  const [cvc2, setCvc2] = useState('')

  // ** Hook
  const bgColors = useBgColor()
  const handleClickOpen = () => {
    setOpen(true)
  }
  return (
    <Card>
      <Box sx={{ justifyContent: 'space-between', gap: '4px', width: '600px' }}>
        <FormControl fullWidth sx={{ display: 'flex', justifyContent: 'space-between' }}>
          <TextField label='Search' placeholder={<Icon icon='mdi:search' />} size='small' />
        </FormControl>
       
      </Box>
    </Card>
  )
}

export default RequestSearchBar
