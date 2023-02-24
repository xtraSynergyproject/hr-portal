// ** MUI Imports
import Box from '@mui/material/Box'
import Input from '@mui/material/Input'

import IconButton from '@mui/material/IconButton'

const Searchbar = props => {
  const { handleLeftSidebarToggle } = props

  return (
    <Box sx={{ pr: 48 }}>
      <Box sx={{ display: 'flex', alignItems: 'center', justifyContent: 'right', width: '100%' }}>
        {<IconButton onClick={handleLeftSidebarToggle} sx={{ mr: 1, ml: -2 }}></IconButton>}
        <Input placeholder='Search items' sx={{ width: '100%', '&:before, &:after': { display: 'none' } }} />
      </Box>
    </Box>
  )
}

export default Searchbar
