// ** React Imports
import { useState, useEffect } from 'react'

// ** Next Import
import { useRouter } from 'next/router'

// ** MUI Components
import Tab from '@mui/material/Tab'
import Box from '@mui/material/Box'
import Grid from '@mui/material/Grid'
import TabPanel from '@mui/lab/TabPanel'
import TabContext from '@mui/lab/TabContext'
import Typography from '@mui/material/Typography'
import { styled } from '@mui/material/styles'
import useMediaQuery from '@mui/material/useMediaQuery'
import MuiTabList from '@mui/lab/TabList'
import CircularProgress from '@mui/material/CircularProgress'

// ** Icon Imports
import Icon from 'src/@core/components/icon'

// ** Demo Components
import Teams from 'src/views/pages/user-profile/teams'
import Profile from 'src/views/pages/user-profile/overview'
import Projects from 'src/views/pages/user-profile/tasks'
import Connections from 'src/views/pages/user-profile/settings'

import Empprofile from '../../../views/dashboards/employess-profile/info/Emprofile'
import AssignmentForm from '../../../views/dashboards/employess-profile/AssignmentForm'
//import Personalinfo from '../../../views/dashboards/employess-profile/personalInfo'
import EmployeeProfiletabs from '../../../views/dashboards/employess-profile/EmployeeProfiletabs'

const TabList = styled(MuiTabList)(({ theme }) => ({
  '& .MuiTabs-indicator': {
    display: 'none'
  },
  '& .Mui-selected': {
    backgroundColor: theme.palette.primary.main,
    color: `${theme.palette.common.white} !important`
  },
  '& .MuiTab-root': {
    minWidth: 65,
    minHeight: 38,
    paddingTop: theme.spacing(2.5),
    paddingBottom: theme.spacing(2.5),
    borderRadius: theme.shape.borderRadius,
    [theme.breakpoints.up('sm')]: {
      minWidth: 130
    }
  }
}))

const CRMDasbboard = ({ tab, data }) => {
  // ** State
  const [activeTab, setActiveTab] = useState(tab)
  const [isLoading, setIsLoading] = useState(true)

  // ** Hooks
  const router = useRouter()
  const hideText = useMediaQuery(theme => theme.breakpoints.down('sm'))

  const handleChange = (event, value) => {
    setIsLoading(true)
    setActiveTab(value)
    router
      .push({
        pathname: `/pages/user-profile/${value.toLowerCase()}`
      })
      .then(() => setIsLoading(false))
  }
  useEffect(() => {
    if (data) {
      setIsLoading(false)
    }
  }, [data])
  useEffect(() => {
    if (tab && tab !== activeTab) {
      setActiveTab(tab)
    }
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [tab])

  const tabContentList = {
    profile: <Profile data={data} />,
    teams: <Teams data={data} />,
    projects: <Projects data={data} />,
    connections: <Connections data={data} />
  }

  return (
    <Grid container spacing={6}>
      <Grid item xs={12}>
        <Empprofile />
      </Grid>
   
      <Grid item xs={12}>
        <EmployeeProfiletabs/>
      </Grid>
     
     
    </Grid>

 
  )
}

export default CRMDasbboard
