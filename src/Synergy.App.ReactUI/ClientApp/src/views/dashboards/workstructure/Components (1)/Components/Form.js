// ** React Imports
import { useState } from 'react'

// ** MUI Imports
import Tab from '@mui/material/Tab'
import TabPanel from '@mui/lab/TabPanel'
import TabContext from '@mui/lab/TabContext'
import { styled } from '@mui/material/styles'
import Typography from '@mui/material/Typography'
import MuiTabList from '@mui/lab/TabList'
import MyForm from './MyForm'
import CancelIcon from '@mui/icons-material/Cancel'
// import FilePresentIcon from '@mui/icons-material/FilePresent'
// import StyleIcon from '@mui/icons-material/Style'
// import TextareaVariant from 'src/views/forms/form-elements/textarea/TextareaVariant'
// import TextareaBasic from 'src/views/forms/form-elements/textarea/TextareaBasic'
// import InputMaskExamples from 'src/views/forms/form-elements/input-mask/InputMaskExamples'
// import CleaveWrapper from 'src/@core/styles/libs/react-cleave'

// Styled TabList component
const TabList = styled(MuiTabList)(({ theme }) => ({
  '& .MuiTabs-indicator': {
    display: 'none'
  },
  '& .Mui-selected': {
    backgroundColor: theme.palette.primary.main,
    color: `${theme.palette.common.white} !important`
  },
  '& .MuiTab-root': {
    minHeight: 38,
    minWidth: 130,
    borderRadius: theme.shape.borderRadius
  }
}))

const TabsCustomized = () => {
  // ** State
  const [value, setValue] = useState('1')

  const handleChange = (event, newValue) => {
    setValue(newValue)
  }

  return (
    <TabContext value={value}>
      <TabList onChange={handleChange} aria-label='customized tabs example'>
        <Tab value='1' label='Create' />
        
      </TabList>
      <TabPanel value='1'>
        <Typography>
        {/* <TextareaVariant /> */}
        {/* <TextareaBasic /> */}
        {/* <CleaveWrapper/> */}
        <MyForm/>
        {/* <InputMaskExamples /> */}
        </Typography>
      </TabPanel>
      
      
    </TabContext>
  )
}

export default TabsCustomized
