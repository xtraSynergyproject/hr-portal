import {
  Box,
  Button,
  Drawer,
  FormControl,
  FormControlLabel,
  InputLabel,
  MenuItem,
  Select,
  Switch,
  Typography
} from '@mui/material'
import FolderIcon from '@mui/icons-material/Folder'
import RefreshIcon from '@mui/icons-material/Refresh'
import { Archive, ContentCopy, ContentCut, Delete } from '@mui/icons-material'
import { useState } from 'react'
import AddWorkspace from '../add-workspace/AddWorkspace'

const Header = () => {
  let [view, setView] = useState('documentView')
  let [openDrawer, setOpenDrawer] = useState(false)

  const handleView = event => {
    setView(event.target.value)
  }

  const toggleDrawer = action => {
    setOpenDrawer(action)
  }

  return (
    //  Header Container
    <Box
      sx={{
        display: 'flex',
        marginBottom: '20px',
        padding: 2,
        gap: '50px',
        justifyContent: 'space-between'
      }}
    >
      {/* Buttons Container  */}
      <Box sx={{ display: 'flex', gap: '10px', flexDirection: 'column' }}>
        {/* New workspace and Refresh button container  */}
        <Box sx={{ display: 'flex', gap: '10px' }}>
          <Button variant='outlined' size='small' startIcon={<FolderIcon />} onClick={() => toggleDrawer(true)}>
            New Workspace
          </Button>
          <Drawer anchor='right' open={openDrawer} onClose={() => toggleDrawer(false)}>
            <AddWorkspace toggleDrawer={toggleDrawer} />
          </Drawer>
          <Button variant='outlined' size='small' startIcon={<RefreshIcon />}>
            Refresh
          </Button>
        </Box>

        {/* File or folder buttons container  */}
        <Box sx={{ display: 'flex', gap: '10px', alignItems: 'center' }}>
          <Button variant='outlined' size='small' startIcon={<ContentCopy />}>
            Copy
          </Button>
          <Button variant='outlined' size='small' startIcon={<ContentCut />}>
            Cut
          </Button>
          <Button variant='outlined' size='small' startIcon={<Delete />}>
            Delete
          </Button>
          <Button variant='outlined' size='small' startIcon={<Archive />}>
            Archive
          </Button>
        </Box>
      </Box>

      <Box
        sx={{
          display: 'flex',
          gap: {
            xs: '20px',
            sm: '20px',
            xl: '10px',
            md: '10px',
            lg: '10px'
          },
          alignItems: 'center',
          flexDirection: {
            xs: 'column',
            sm: 'column',
            xl: 'row',
            md: 'row',
            lg: 'row'
          },
          justifyContent: 'flex-end'
        }}
      >
        <Box>
          <FormControlLabel
            value='top'
            control={<Switch color='primary' />}
            label='Enable Version'
            labelPlacement='start'
          />
        </Box>

        <Box sx={{ minWidth: 120 }}>
          <FormControl fullWidth>
            <InputLabel id='select-label'>Select View Type</InputLabel>
            <Select labelId='select-label' id='simple-select' value={view} label='view' onChange={handleView}>
              <MenuItem value={'documentView'}>Document View</MenuItem>
              <MenuItem value={'fileView'}>File View</MenuItem>
              <MenuItem value={'folderView'}>Folder View</MenuItem>
              <MenuItem value={'bookView'}>Book View</MenuItem>
            </Select>
          </FormControl>
        </Box>
      </Box>
    </Box>
  )
}

export default Header
