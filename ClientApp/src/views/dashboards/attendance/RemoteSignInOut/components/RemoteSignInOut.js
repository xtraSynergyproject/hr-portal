import React from 'react'
import RemoteTable from './RemoteTable'
import UserProfile from 'src/views/dashboards/payroll/components/UserProfile'
import TerminationRequestModal from '../../../hr-only/termination/components/TerminationRequestModal'
//import TerminationRequestModal from './components/RemoteButton'
 //import RemoteButton from './components/RemoteButton'
//import TerminationRequestModal from '../../hr-only/termination/components/'
import RemoteModal from './RemoteModal'
import SearchButton from './SearchButton'

function index() {
  return (
    <>
    <div >
      <SearchButton/>
    <UserProfile/>
    <RemoteModal/>
    <RemoteTable/>
    </div>
    </>
  )
}

export default index